﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NOR_WAY.Model;

namespace NOR_WAY.DAL
{
    public class BussRepository : IBussRepository
    {
        private readonly BussContext _db;

        public BussRepository(BussContext db)
        {
            _db = db;
        }

        public async Task<Avgang> FinnNesteAvgang(AvgangParam param)
        {

            Stopp startStopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == param.StartStopp);
            Stopp sluttStopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == param.StartStopp);

            // Finner alle Rutene som inkluderer påstigning og som inkluderer avstigning
            List<Ruter> startStoppRuter = await FinnRuter(startStopp);
            List<Ruter> sluttStoppRuter = await FinnRuter(sluttStopp);

            // Finner alle rutene påstigning og avstigning har til felles
            Ruter fellesRute = await FinnFellesRute(startStoppRuter, sluttStoppRuter);

            // Finne ut hvilket stoppNummer påstigning og avstigning har i den felles ruten
            int stoppNummer1 = await FinnStoppNummer(startStopp, fellesRute);
            int stoppNummer2 = await FinnStoppNummer(sluttStopp, fellesRute);

            // Beregner reisetiden fra stopp 1 til stopp 2
            int reisetid = await BeregnReisetid(stoppNummer1, stoppNummer2);

            // Finne neste avgang ved å søke i databasen to ulike måter basert på brukerinput: 
            // Finne ut hvordan å håndtere dato og tidspunkter mtp sammenligning av verdier fra brukerinput og i databasen
            string innAvreise = param.Dato + " " + param.Tidspunkt;
            DateTime avreise = DateTime.ParseExact(innAvreise, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);

            Avganger nesteAvgang = new Avganger();
            if (param.AvreiseEtter == true) // Hvis Avreise Etter:
            {
                nesteAvgang = await _db.Avganger
                    .Where(a => a.Rute == fellesRute)
                    .SingleOrDefaultAsync(a => a.Avreise >= avreise);
            }
            else  // Hvis Ankomst Før:
            {
                DateTime ankomst = avreise.AddMinutes(-reisetid);
                nesteAvgang = await _db.Avganger
                    .Where(a => a.Rute == fellesRute)
                    .SingleOrDefaultAsync(a => a.Avreise <= ankomst);
            }

            // Opretter Avgang-objektet som skal sendes til klienten
            string utAvreise = nesteAvgang.Avreise.ToString("yyyy-MM-dd HH:mm");
            string utAnkomst = nesteAvgang.Avreise.AddMinutes(reisetid).ToString("yyyy-MM-dd HH:mm");
            Avgang utAvgang = new Avgang
            {
                AvgangId = nesteAvgang.Id,
                Rutenavn = fellesRute.Rutenavn,
                Linjekode = fellesRute.Linjekode,
                // TODO: Beregn pris, basert på startpris og antall stopp
                Pris = 100,
                Avreise = utAvreise,
                Ankomst = utAnkomst,
                Reisetid = reisetid
            };

            Console.Write("Utskrift: " + utAvgang.AvgangId);
            return utAvgang;
        }

        /* Hjelpemetode som tar inn et stopp object og returnerer en
        liste med ruter som innholder stoppet */
        private async Task<List<Ruter>> FinnRuter(Stopp stopp)
        {
            return await _db.RuteStopp.Where(rs => rs.Stopp == stopp).Select(rs => rs.Rute).ToListAsync();
        }

        // Hjelpemetode som finner Rutern to lister med Ruter har til felles
        private async Task<Ruter> FinnFellesRute(List<Ruter> startStoppRuter, List<Ruter> sluttStoppRuter)
        {
            Ruter fellesRute = new Ruter();
            foreach (Ruter startStoppRute in startStoppRuter)
            {
                foreach (Ruter sluttStoppRute in sluttStoppRuter)
                {
                    if (startStoppRute == sluttStoppRute)
                    {
                        fellesRute = startStoppRute;
                    }
                }
            }

            return fellesRute;
        }

        // Hjelpemetode som finner stoppnummeret til et spesifikt stopp i en spesifikk rute
        private async Task<int> FinnStoppNummer(Stopp startStopp, Ruter fellesRute)
        {
            RuteStopp ruteStopp = await _db.RuteStopp.FirstOrDefaultAsync(
                        rs => rs.Stopp == startStopp && rs.Rute == fellesRute);

            return ruteStopp.StoppNummer;
        }

        // Hjelpemetode som beregner reisetiden fra startStopp til sluttStopp
        private async Task<int> BeregnReisetid(int startStopp, int sluttStopp)
        {
            // Beregner reisetid
            List<int> minTilNesteStoppList = await _db.RuteStopp
                .Where(rs => rs.StoppNummer > startStopp && rs.StoppNummer < sluttStopp)
                .Select(rs => rs.MinutterTilNesteStopp).ToListAsync();

            int reisetid = 0;
            foreach (int minutter in minTilNesteStoppList)
            {
                reisetid += minutter;
            }
            return reisetid;
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
