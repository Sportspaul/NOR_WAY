using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace NOR_WAY.DAL.Repositories
{
    [ExcludeFromCodeCoverage]
    public class HjelpeRepository
    {
        private readonly BussContext _db;
        private readonly ILogger _log;

        public HjelpeRepository(BussContext db, ILogger log)
        {
            _db = db;
            _log = log;
        }

        /* Hjelpemetode som tar inn et Stopp-objekt og returnerer en
        liste med ruter som innholder stoppet */

        public async Task<List<Ruter>> FinnRuteneTilStopp(Stopp stopp)
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
        public Ruter FinnFellesRute(List<Ruter> startStoppRuter, List<Ruter> sluttStoppRuter)
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
        public async Task<int> FinnStoppNummer(Stopp stopp, Ruter fellesRute)
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
        public async Task<int> BeregnReisetid(int startStopp, int sluttStopp, Ruter fellesRute)
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
        public DateTime StringTilDateTime(string dato, string tidspunkt)
        {
            string innTidStreng = dato + " " + tidspunkt;
            return DateTime.ParseExact(innTidStreng, "yyyy-MM-dd HH:mm",
                CultureInfo.InvariantCulture);
        }

        // Hjelpemetode som finner neste avgang som passer for brukeren
        public async Task<Avganger> NesteAvgang(Ruter fellesRute, int reisetid,
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

        public async Task<int> BeregnPris(Ruter rute, int antallStopp, List<string> billettyper)
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
        public async Task<DateTime> BeregnAvreisetid(DateTime avreise, int stoppNummer, Ruter fellesRute)
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
    }
}