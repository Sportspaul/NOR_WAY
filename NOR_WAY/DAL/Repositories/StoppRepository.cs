using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    public class StoppRepository : IStoppRepository
    {
        private readonly BussContext _db;
        private readonly ILogger<StoppRepository> _log;
        private readonly HjelpeRepository _hjelp;

        public StoppRepository(BussContext db, ILogger<StoppRepository> log)
        {
            _hjelp = new HjelpeRepository(db, log);
            _db = db;
            _log = log;
        }

        public async Task<List<Stopp>> FinnMuligeStartStopp(StoppModel startStopp)
        {
            try
            {
                Stopp stopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == startStopp.Navn);
                List<Ruter> ruter = await _hjelp.FinnRuteneTilStopp(stopp);

                List<Stopp> stoppListe = new List<Stopp>();
                foreach (Ruter rute in ruter)
                {
                    int stoppNummer = await _hjelp.FinnStoppNummer(stopp, rute);
                    List<Stopp> tempListe = await _db.RuteStopp
                        .Where(rs => rs.StoppNummer < stoppNummer && rs.Rute == rute)
                        .Select(rs => rs.Stopp).ToListAsync();
                    stoppListe.AddRange(tempListe);
                }

                return stoppListe;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<List<Stopp>> FinnMuligeSluttStopp(StoppModel sluttStopp)
        {
            try
            {
                Stopp stopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == sluttStopp.Navn);
                List<Ruter> ruter = await _hjelp.FinnRuteneTilStopp(stopp);

                List<Stopp> stoppListe = new List<Stopp>();
                foreach (Ruter rute in ruter)
                {
                    int stoppNummer = await _hjelp.FinnStoppNummer(stopp, rute);
                    List<Stopp> tempListe = await _db.RuteStopp
                        .Where(rs => rs.StoppNummer > stoppNummer && rs.Rute == rute)
                        .Select(rs => rs.Stopp).ToListAsync();
                    stoppListe.AddRange(tempListe);
                }

                return stoppListe;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<List<Stopp>> HentAlleStopp()
        {
            try
            {
                List<Stopp> alleStopp = await _db.Stopp.Select(s => new Stopp
                {
                    Id = s.Id,
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

        public Task<List<StoppMedLinjekoder>> HentAlleStoppMedRuter()
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterStoppnavn(Stopp innStopp)
        {
            throw new NotImplementedException();
        }
    }
}
