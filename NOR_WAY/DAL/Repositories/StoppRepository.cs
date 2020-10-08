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
        private ILogger<StoppRepository> _log;

        public StoppRepository(BussContext db, ILogger<StoppRepository> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<List<Stopp>> FinnMuligeStartStopp(StoppModel startStopp)
        {
            try
            {
                Stopp stopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == startStopp.Navn);
                List<Ruter> ruter = await FinnRuter(stopp);

                List<Stopp> stoppListe = new List<Stopp>();
                foreach (Ruter rute in ruter)
                {
                    int stoppNummer = await FinnStoppNummer(stopp, rute);
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
                List<Ruter> ruter = await FinnRuter(stopp);

                List<Stopp> stoppListe = new List<Stopp>();
                foreach (Ruter rute in ruter)
                {
                    int stoppNummer = await FinnStoppNummer(stopp, rute);
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

        // TODO: Duplikatkode fra BussReiseRepository, finn en løsning
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

        // TODO: Duplikatkode fra BussReiseRepository, finn en løsning
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

        public Task<bool> EndreStoppnavn(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
