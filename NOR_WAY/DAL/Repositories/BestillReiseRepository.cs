﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BestillReiseRepository : IBestillReiseRepository
    {
        private readonly BussContext _db;
        private ILogger<BestillReiseRepository> _log;

        public BestillReiseRepository(BussContext db, ILogger<BestillReiseRepository> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<Reisedetaljer> FinnNesteAvgang(Avgangkriterier input)
        {
            // Henter avgang- og påstigningsstoppet fra DB
            Stopp startStopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == input.StartStopp);
            Stopp sluttStopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == input.SluttStopp);
            if (startStopp.Equals(sluttStopp))
            {
                _log.LogInformation("Sluttstopp kan ikke være lik startstopp!");
                return null;
            }

            // Finner alle Rutene som inkluderer påstigning og som inkluderer avstigning
            List<Ruter> startStoppRuter = await FinnRuter(startStopp);
            List<Ruter> sluttStoppRuter = await FinnRuter(sluttStopp);

            // Finner ruten påstigning og avstigning har til felles
            Ruter fellesRute = FinnFellesRute(startStoppRuter, sluttStoppRuter);
            if (fellesRute == null) { return null; } // Hvis stoppene ikke har noen felles ruter

            // Finne ut hvilket stoppNummer påstigning og avstigning har i den felles ruten
            int stoppNummer1 = await FinnStoppNummer(startStopp, fellesRute);
            int stoppNummer2 = await FinnStoppNummer(sluttStopp, fellesRute);
            // Hvis første stopp kommer senere i ruten enn siste stopp
            if (stoppNummer1 > stoppNummer2)
            {
                _log.LogInformation("Seneste stopp har ikke lavere stoppnummer enn tidligste stopp!");
                return null;
            }

            int reisetid = await BeregnReisetid(stoppNummer1, stoppNummer2, fellesRute); // Beregner reisetiden fra stopp påstigning til avstigning
            int antallBilletter = input.Billettyper.Count();    // antall billetter brukeren ønsker
            DateTime innTid = StringTilDateTime(input.Dato, input.Tidspunkt);   // Konverterer fra strings til DateTime

            // Finne neste avgang som passer, basert på brukerens input
            Avganger nesteAvgang = await NesteAvgang(fellesRute, reisetid, input.AvreiseEtter, innTid, antallBilletter);
            if (nesteAvgang == null) { return null; }  // Hvis ingen avgang ble funnet

            // Beregner avreise og ankomst
            DateTime avreise = await BeregnAvreisetid(nesteAvgang.Avreise, stoppNummer1, fellesRute);
            DateTime ankomst = avreise.AddMinutes(reisetid);

            // Konverterer avreise og ankomst fra DateTime til en strings
            string utAvreise = avreise.ToString("dd-MM-yyyy HH:mm");
            string utAnkomst = ankomst.ToString("dd-MM-yyyy HH:mm");

            // Beregner prisen basert på startpris og antall stopp
            int antallStopp = stoppNummer2 - stoppNummer1;
            int pris = await BeregnPris(fellesRute, antallStopp, input.Billettyper);

            // Opretter Avgang-objektet som skal sendes til klienten
            Reisedetaljer utAvgang = new Reisedetaljer
            {
                AvgangId = nesteAvgang.Id,
                Rutenavn = fellesRute.Rutenavn,
                Linjekode = fellesRute.Linjekode,
                Pris = pris,
                Avreise = utAvreise,
                Ankomst = utAnkomst,
                Reisetid = reisetid
            };

            return utAvgang;
        }

        /* Hjelpemetode som tar inn et Stopp-objekt og returnerer en
        liste med ruter som innholder stoppet */

        private async Task<List<Ruter>> FinnRuter(Stopp stopp)
        {
            try
            {
                return await _db.RuteStopp
                    .Where(rs => rs.Stopp == stopp)
                    .Select(rs => rs.Rute)
                    .ToListAsync();
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        // Hjelpemetode som finner Rutern to lister med Ruter har til felles
        private Ruter FinnFellesRute(List<Ruter> startStoppRuter, List<Ruter> sluttStoppRuter)
        {
            if (startStoppRuter == null || sluttStoppRuter == null)
            {
                _log.LogInformation(" Ett eller begge oppgitte stopp har ingen rute");
                return null;
            }
            Ruter fellesRute = new Ruter();
            foreach (Ruter startStoppRute in startStoppRuter)
            {
                foreach (Ruter sluttStoppRute in sluttStoppRuter)
                {
                    if (startStoppRute.Linjekode == sluttStoppRute.Linjekode)
                    {
                        fellesRute = startStoppRute;
                        return fellesRute;
                    }
                }
            }
            return fellesRute;
        }

        // Hjelpemetode som finner stoppnummeret til et spesifikt stopp i en spesifikk rute
        private async Task<int> FinnStoppNummer(Stopp stopp, Ruter fellesRute)
        {
            try
            {
                RuteStopp ruteStopp = await _db.RuteStopp
              .FirstOrDefaultAsync(rs => rs.Stopp == stopp && rs.Rute == fellesRute);
                if (ruteStopp == null)
                {
                    _log.LogInformation("Stoppet er ikke på ruten");
                    return -1;
                }
                return ruteStopp.StoppNummer;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return -1;
            }
        }

        // Hjelpemetode som beregner reisetiden fra startStopp til sluttStopp
        private async Task<int> BeregnReisetid(int startStopp, int sluttStopp, Ruter fellesRute)
        {
            try
            {
                List<int> minTilNesteStoppList = await _db.RuteStopp
               .Where(rs => rs.StoppNummer >= startStopp && rs.StoppNummer < sluttStopp && rs.Rute == fellesRute)
               .Select(rs => rs.MinutterTilNesteStopp)
               .ToListAsync();

                int reisetid = 0;
                foreach (int minutter in minTilNesteStoppList)
                {
                    reisetid += minutter;
                }
                return reisetid;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return -1;
            }
        }

        // Oversetter string verdiene av dato og tidspunkt til et DateTime-objekt
        private DateTime StringTilDateTime(string dato, string tidspunkt)
        {
            string innTidStreng = dato + " " + tidspunkt;
            return DateTime.ParseExact(innTidStreng, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture);
        }

        // Hjelpemetode som finner neste avgang som passer for brukeren
        private async Task<Avganger> NesteAvgang(Ruter fellesRute, int reisetid,
            bool avreiseEtter, DateTime innTid, int antallBilletter)
        {
            try
            {
                List<Avganger> muligeAvganger = new List<Avganger>();
                // Hvis "Avreise Etter" er valgt av brukeren
                if (avreiseEtter)
                {
                    // Henter alle avganger hvor brukeren kan reiser etter innTid
                    muligeAvganger = await _db.Avganger
                        .Where(a => a.Rute == fellesRute && a.Avreise >= innTid).ToListAsync();
                }
                // Hvis "Ankomst Før" er valgt av brukeren
                else
                {
                    // Trekker fra reisetid på innTid for å finne ut når bruker må reise for å komme fram i tide
                    DateTime avreise = innTid.AddMinutes(-reisetid);

                    // Henter alle avganger hvor brukeren kan kommer fram før innTid
                    muligeAvganger = await _db.Avganger
                        .Where(a => a.Rute == fellesRute && a.Avreise <= avreise)
                        .ToListAsync();
                }
                if (muligeAvganger.Count > 0)                                                          // Hvis listen med mulige avganger ikke er tom
                {
                    Avganger nesteAvgang = muligeAvganger[0];
                    TimeSpan lavesteDiff = innTid.Subtract(muligeAvganger[0].Avreise).Duration();      // Føste avgang i listen sitt avvik fra ønsket tid
                    foreach (Avganger muligAvgang in muligeAvganger)                                     // Looper gjennom alle avgangene
                    {
                        TimeSpan diff = innTid.Subtract(muligAvgang.Avreise).Duration();
                        if (diff < lavesteDiff)                                                         // Hvis differansen fra ønsket tid er mindre enn hittil laveste differanse
                        {
                            if (muligAvgang.SolgteBilletter + antallBilletter <= fellesRute.Kapasitet)  // Hvis det er nok billetter igjen
                            {
                                nesteAvgang = muligAvgang;
                                lavesteDiff = diff;
                            }
                        }
                    }

                    return nesteAvgang;
                }
                else
                {
                    _log.LogInformation("Finner ingen avganger");
                    return null;
                }
            }
            catch (ArgumentOutOfRangeException bound)
            {
                _log.LogInformation(bound.Message);
                return null;
            }
        }

        private async Task<int> BeregnPris(Ruter rute, int antallStopp, List<string> billettyper)
        {
            try
            {
                // Finner den standard billettpris for en reise
                int startpris = rute.Startpris;
                int tilleggPerStopp = rute.TilleggPerStopp;
                int maxPris = startpris + (tilleggPerStopp * antallStopp);

                // Henter liste med alle billettyper i DB
                List<Billettyper> billettypeListe = await _db.Billettyper.Select(b => new Billettyper
                {
                    Billettype = b.Billettype,
                    Rabattsats = b.Rabattsats
                }).ToListAsync();

                // Finner rabattsatsen for en billett
                var rabbattsatser = new List<int>();
                foreach (string billettype in billettyper)
                {
                    int i = 0;
                    while (billettypeListe[i].Billettype != billettype)
                    {
                        i++;
                    }
                    rabbattsatser.Add(billettypeListe[i].Rabattsats);
                }

                // Finne totalpris
                double totalpris = 0;
                foreach (int rabbattsats in rabbattsatser)
                {
                    double billettPris = maxPris * (1 - ((double)rabbattsats / 100)); // eks. 1 - 0.25 = 0.75
                    totalpris += billettPris;
                }

                return (int)Math.Round(totalpris);
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return -1;
            }
        }

        // Endrer avreisetiden hvis påstigning ikke er første stopp i ruten
        private async Task<DateTime> BeregnAvreisetid(DateTime avreise, int stoppNummer, Ruter fellesRute)
        {
            // Hvis påstigning er første stopp i ruten endres ikke avreise
            if (stoppNummer == 1)
            {
                return avreise;
            }
            else
            {
                List<int> minTilNesteStoppList = await _db.RuteStopp
                .Where(rs => rs.StoppNummer < stoppNummer && rs.Rute == fellesRute)
                .Select(rs => rs.MinutterTilNesteStopp)
                .ToListAsync();
                int totalTid = 0;
                foreach (int minutter in minTilNesteStoppList)
                {
                    totalTid += minutter;
                }
                return avreise.AddMinutes(totalTid);
            }
        }

        // Fullfør ordre
        public async Task<bool> FullforOrdre(OrdreModel kundeOrdreParam)
        {
            try
            {
                // Henter ut ruten som tilhører kundeOrdreParam
                Ruter rute = await _db.Ruter.FirstOrDefaultAsync(r => r.Linjekode == kundeOrdreParam.Linjekode);

                // Henter Avgangens Id
                Avganger avgang = await _db.Avganger.FirstOrDefaultAsync(a => a.Id == kundeOrdreParam.AvgangId);

                // Finner startStopp, og finner stoppnummeret i ruten
                Stopp startStopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == kundeOrdreParam.StartStopp);
                int stoppNummer1 = await FinnStoppNummer(startStopp, rute);

                // Finner sluttStopp, og finner stoppnummeret i ruten
                Stopp sluttStopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == kundeOrdreParam.SluttStopp);
                int stoppNummer2 = await FinnStoppNummer(sluttStopp, rute);

                // Regner ut antall stopp
                int antallStopp = stoppNummer2 - stoppNummer1;

                // Finner summen for reisen
                // antallStopp, rute, liste med billettype
                int sum = await BeregnPris(rute, antallStopp, kundeOrdreParam.Billettyper);

                // Lager en ordre basert på kundeOrdreParam, rute og avgang
                var ordre = new Ordre
                {
                    Epost = kundeOrdreParam.Epost,
                    StartStopp = startStopp,
                    SluttStopp = sluttStopp,
                    Sum = sum,
                    Rute = rute,
                    Avgang = avgang
                };

                // Legger ordren til i databasen
                _db.Ordre.Add(ordre);

                // Raden til spesifisert avgang
                Avganger dbAvgang = _db.Avganger.Find(avgang.Id);

                // Går gjennom listen med billettyper
                foreach (string billettype in kundeOrdreParam.Billettyper)
                {
                    // Henter ut en billettype i listen
                    Billettyper billettypeObjekt = await _db.Billettyper.FirstOrDefaultAsync(a => a.Billettype == billettype);

                    // Lager en ordrelinje
                    var ordrelinje = new Ordrelinjer
                    {
                        Billettype = billettypeObjekt,
                        Ordre = ordre
                    };

                    // Legger denne ordrelinjen til databasen
                    _db.Ordrelinjer.Add(ordrelinje);

                    // Øker antalll solgte billetter med 1
                    dbAvgang.SolgteBilletter++;
                }

                // Lagrer alt som er blitt lagt til i databasen
                _db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }
    }
}