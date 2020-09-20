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
            int pris = BeregnPris(fellesRute, antallStopp);

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

        // Bregener fullpris av hva en billett vil koste 
        private int BeregnPris(Ruter fellesRute, int antallStopp)
        {
            int startpris = fellesRute.Startpris;
            int tilleggPerStopp = fellesRute.TilleggPerStopp;
            int totalpris = startpris + tilleggPerStopp * antallStopp;

            return totalpris;
        }

        // Endrer avreisetiden hvis påstigning ikke er første stopp i ruten
        private async Task<DateTime> BeregnAvreisetid(DateTime avreise, int stoppNummer, Ruter fellesRute)
        {

            // Hvis påstigning er første stopp i ruten endres ikke avreise
            if (stoppNummer == 1)
            {
                return avreise;
            }
            else {
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

        
        public async Task<bool> FullforOrdre(KundeOrdre ordre)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Billettyper>> HentAlleBillettyper()
        {
            List<Billettyper> alleBillettyper = await _db.Billettyper.Select(b => new Billettyper
            {
                Billettype = b.Billettype,
                Rabattsats = b.Rabattsats
            }).ToListAsync();

            return alleBillettyper;
        }

        public async Task<List<Stopp>> HentAlleStopp()
        {
            List<Stopp> alleStopp = await _db.Stopp.Select(s => new Stopp
            {
                Navn = s.Navn
            }).ToListAsync();

            return alleStopp;
        }
    }
}
