﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    public class RuteStoppRepository : IRuteStoppRepository
    {
        private readonly BussContext _db;
        private readonly ILogger<RuteStoppRepository> _log;

        public RuteStoppRepository(BussContext db, ILogger<RuteStoppRepository> log)
        {
            _db = db;
            _log = log;
        }

        public Task<bool> FjernRuteStopp(int stoppNummer, string linjekode)
        {
            throw new NotImplementedException();
        }

        public Task<List<RuteStoppModel>> HentAlleRuteStopp()
        {
            throw new NotImplementedException();
        }

        public async Task<List<RuteStoppModel>> HentRuteStopp(string linjekode)
        {
            try
            {
                List<RuteStopp> ruteStopp = await _db.RuteStopp.Where(rs => rs.Rute.Linjekode == linjekode).ToListAsync();
                List<RuteStoppModel> utRuteStopp = new List<RuteStoppModel>();
                foreach (RuteStopp rs in ruteStopp)
                {
                    RuteStoppModel rsm = new RuteStoppModel
                    {
                        StoppNummer = rs.StoppNummer,
                        Stoppnavn = rs.Stopp.Navn,
                        MinutterTilNesteStopp = rs.MinutterTilNesteStopp,
                        Linjekode = rs.Rute.Linjekode
                    };
                    utRuteStopp.Add(rsm);
                }
                return utRuteStopp;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<bool> NyRuteStopp(RuteStoppModel innRuteStopp)
        {
            try
            {
                // Henter alle RuteStopp fra samme rute som har likt eller høyre stoppnummer enn det nye stoppet
                List<RuteStopp> itererStoppNummer = await _db.RuteStopp
                    .Where(rs => rs.StoppNummer >= innRuteStopp.StoppNummer && rs.Rute.Linjekode == innRuteStopp.Linjekode).ToListAsync();

                // Adderer alle med stoppnummer som er større eller lik det nye rutestoppet med 1
                foreach (RuteStopp rs in itererStoppNummer)
                {
                    rs.StoppNummer++;
                }

                // Henter ruten til det nye RuteStopp-objektet
                Ruter rute = await _db.Ruter.FindAsync(innRuteStopp.Linjekode);

                // Nytt RuteStopp-objekt
                RuteStopp nyttRuteStopp = new RuteStopp
                {
                    StoppNummer = innRuteStopp.StoppNummer,
                    MinutterTilNesteStopp = innRuteStopp.MinutterTilNesteStopp,
                    Rute = rute,
                };

                // Sjekker om det allerede eksisterer et stopp med tilsvarende navn i DB
                Stopp eksisterendeStopp = await _db.Stopp
                    .Where(s => s.Navn == innRuteStopp.Stoppnavn).SingleOrDefaultAsync();

                // Hvis det eksiterer blir dette Stopp-objektet brukt
                if (eksisterendeStopp != null)
                {
                    nyttRuteStopp.Stopp = eksisterendeStopp;
                }
                // Hvis det ikke eksiterer blir et nytt Stopp-okbjekt lagt til 
                else
                {
                    Stopp nyttStopp = new Stopp { Navn = innRuteStopp.Stoppnavn };
                    nyttRuteStopp.Stopp = nyttStopp;
                }

                _db.RuteStopp.Add(nyttRuteStopp);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }


        }

        public Task<bool> OppdaterRuteStopp(int stoppNumer, string linjekode, RuteStoppModel oppdatertRuteStopp)
        {
            throw new NotImplementedException();
        }
    }
}
