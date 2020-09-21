﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NOR_WAY.Model;

namespace NOR_WAY.DAL
{
    public class BussRepository : IBussRepository
    {
        private readonly BussContext _db;

        private ILogger<BussRepository> _log;

        public BussRepository(BussContext db, ILogger<BussRepository> log )
        {
            _db = db;
            _log = log;
        }

        public async Task<Avgang> FinnNesteAvgang(AvgangParam input)
        {
            // Henter avgang- og påstignings-stoppet fra DB
            Stopp startStopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == input.StartStopp);
            Stopp sluttStopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == input.SluttStopp);
            if (startStopp.Equals(sluttStopp)) { return null; }

            // Finner alle Rutene som inkluderer påstigning og som inkluderer avstigning
            List<Ruter> startStoppRuter = await FinnRuter(startStopp);
            List<Ruter> sluttStoppRuter = await FinnRuter(sluttStopp);

            // Finner ruten påstigning og avstigning har til felles
            Ruter fellesRute = FinnFellesRute(startStoppRuter, sluttStoppRuter);
            if (fellesRute == null) { return null; }

            // Finne ut hvilket stoppNummer påstigning og avstigning har i den felles ruten
            int stoppNummer1 = await FinnStoppNummer(startStopp, fellesRute);
            int stoppNummer2 = await FinnStoppNummer(sluttStopp, fellesRute);
            if (stoppNummer1 > stoppNummer2) { return null; }

            // Beregner reisetiden fra stopp påstigning til avstigning
            int reisetid = await BeregnReisetid(stoppNummer1, stoppNummer2, fellesRute);

            // Finne neste avgang som passer, basert på brukerens input
            Avganger nesteAvgang = await NesteAvgang(fellesRute, reisetid,
            input.AvreiseEtter, input.Dato, input.Tidspunkt);

            // Beregner avreise og ankomst
            DateTime avreise = await BeregnAvreisetid(nesteAvgang.Avreise, stoppNummer1, fellesRute);
            DateTime ankomst = avreise.AddMinutes(reisetid);

            // Konverterer avreise og ankomst fra DateTime til en strings
            string utAvreise = avreise.ToString("yyyy-MM-dd HH:mm");
            string utAnkomst = ankomst.ToString("yyyy-MM-dd HH:mm"); 

            // Beregner prisen basert på startpris og antall stopp
            int antallStopp = stoppNummer2 - stoppNummer1;

            // TODO: Fikse billettyper listen via frontend
            List<string> billettyper = new List<string>();
            billettyper.Add("Student");
            billettyper.Add("Honnør");
            int pris = await BeregnPris(fellesRute, antallStopp, billettyper);

            // Opretter Avgang-objektet som skal sendes til klienten
            Avgang utAvgang = new Avgang
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
            return await _db.RuteStopp
                .Where(rs => rs.Stopp == stopp)
                .Select(rs => rs.Rute)
                .ToListAsync();
        }

        // Hjelpemetode som finner Rutern to lister med Ruter har til felles
        private Ruter FinnFellesRute(List<Ruter> startStoppRuter, List<Ruter> sluttStoppRuter)
        {
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
            RuteStopp ruteStopp = await _db.RuteStopp
                .FirstOrDefaultAsync(rs => rs.Stopp == stopp && rs.Rute == fellesRute);

            return ruteStopp.StoppNummer;
        }

        // Hjelpemetode som beregner reisetiden fra startStopp til sluttStopp
        private async Task<int> BeregnReisetid(int startStopp, int sluttStopp, Ruter fellesRute)
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

        // Hjelpemetode som finner neste avgang som passer for brukeren
        private async Task<Avganger> NesteAvgang(Ruter fellesRute, int reisetid,
            bool avreiseEtter, string dato, string tidspunkt)
        {
            // Oversetter string verdiene av dato og tidspunkt til et DateTime-objekt
            string innAvreise = dato + " " + tidspunkt;
            DateTime avreise = DateTime.ParseExact(innAvreise, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture);

            List<Avganger> kommendeAvganger = new List<Avganger>();
            if (avreiseEtter == true) // Hvis Avreise Etter:
            {
                kommendeAvganger = await _db.Avganger
                    .Where(a => a.Rute == fellesRute && a.Avreise >= avreise).ToListAsync();
            }
            else  // Hvis Ankomst Før:
            {
                DateTime ankomst = avreise.AddMinutes(-reisetid);
                kommendeAvganger = await _db.Avganger
                    .Where(a => a.Rute == fellesRute && a.Avreise <= ankomst)
                    .ToListAsync();
            }

            Avganger nesteAvgang = kommendeAvganger[0];
            TimeSpan lavesteDiff = avreise.Subtract(kommendeAvganger[0].Avreise).Duration();
            for (int i = 1; i < kommendeAvganger.Count; i++)
            {
                TimeSpan diff = avreise.Subtract(kommendeAvganger[i].Avreise).Duration();
                if (diff < lavesteDiff)
                {
                    nesteAvgang = kommendeAvganger[i];
                }
            }

            return nesteAvgang;
        }


        private async Task<int> BeregnPris(Ruter rute, int antallStopp, List<string> billettyper)
        {
            // Finner den voksenbillett pris for en reise
            int startpris = rute.Startpris;
            int tilleggPerStopp = rute.TilleggPerStopp;
            int maxPris = startpris + tilleggPerStopp * antallStopp;

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
            foreach(int rabbattsats in rabbattsatser)
            {
                double billettPris = ((double)maxPris) * (1 - (double)rabbattsats / 100); // 1 - 0.25 = 0.75
                totalpris += billettPris;
            }

            return (int)Math.Round(totalpris);
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
        public async Task<bool> FullforOrdre(KundeOrdre kundeOrdreParam)
        {
            // Henter ut ruten som tilhører kundeOrdreParam
            Ruter rute = await _db.Ruter.FirstOrDefaultAsync(r => r.Linjekode == kundeOrdreParam.Linjekode);
            
            // Henter Avgangens Id
            Avganger avgang = await _db.Avganger.FirstOrDefaultAsync(a => a.Id == kundeOrdreParam.AvgangId);

            // Finner startStopp, og finner stoppnummeret i ruten
            Stopp startStopp = await _db.Stopp.FirstOrDefaultAsync(a => a.Navn == kundeOrdreParam.StartStopp);
            int stoppNummer1 = await FinnStoppNummer(startStopp, rute);

            // Finner sluttStopp, og finner stoppnummeret i ruten
            Stopp sluttStopp = await _db.Stopp.FirstOrDefaultAsync(a => a.Navn == kundeOrdreParam.SluttStopp);
            int stoppNummer2 = await FinnStoppNummer(sluttStopp, rute);

            // Regner ut antall stopp
            int antallStopp = stoppNummer2 - stoppNummer1;

            // Finner summen for reisen
            // antallStopp, rute, liste med billettype
            int sum = await BeregnPris(rute, antallStopp, kundeOrdreParam.Billettype);



            // Lager en ordre basert på kundeOrdreParam, rute og avgang
            var ordre = new Ordre
            {
                Epost = kundeOrdreParam.Epost,
                StartStopp = startStopp,
                SluttStopp = startStopp,
                Sum = sum,
                Rute = rute,
                Avgang = avgang
            };

            // Legger ordren til i databasen
            _db.Ordre.Add(ordre);

            // Går gjennom listen med billettyper
            foreach (string billettype in kundeOrdreParam.Billettype)
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
            }

            //Lagrer alt som er blitt lagt til i databasen
            _db.SaveChanges();

            return true;
        }

        public async Task<List<Billettyper>> HentAlleBillettyper()
        {
            try
            {
                List<Billettyper> alleBillettyper = await _db.Billettyper.Select(b => new Billettyper
                {
                    Billettype = b.Billettype,
                    Rabattsats = b.Rabattsats
                }).ToListAsync();

                return alleBillettyper;
            } catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
            
        }

        public async Task<List<Stopp>> HentAlleStopp()
        {
            try {
                List<Stopp> alleStopp = await _db.Stopp.Select(s => new Stopp
                {
                    Navn = s.Navn
                }).ToListAsync();

                return alleStopp;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }

            
        }
    }
}
