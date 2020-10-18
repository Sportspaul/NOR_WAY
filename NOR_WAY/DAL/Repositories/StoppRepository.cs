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

        public async Task<List<StoppMedLinjekoder>> HentAlleStoppMedRuter()
        {
            try
            {
                List<RuteStopp> rutestopp = await _db.RuteStopp
                        .Select(rs => new RuteStopp
                        {
                            Rute = rs.Rute,
                            Stopp = rs.Stopp
                        }).OrderBy(rs => rs.Stopp.Id).ToListAsync();

                List<StoppMedLinjekoder> stoppMedLinjekoder = new List<StoppMedLinjekoder>();
                for (int i = 0; i < rutestopp.Count(); i++)
                {
                    List<string> linjekoder = new List<string>();
                    //TODO: Feil implementert, rette opp
                    /*
                     * Alle rutestopp med felles stoppID, skal legges inn i et StoppMedLinjekoder-objekt
                     */
                    linjekoder.Add(rutestopp[i].Rute.Linjekode);

                    StoppMedLinjekoder smlk = new StoppMedLinjekoder { Id = rutestopp[i].Stopp.Id, Stoppnavn = rutestopp[i].Stopp.Navn, Linjekoder = linjekoder };
                    stoppMedLinjekoder.Add(smlk);
                }
                return stoppMedLinjekoder;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<bool> OppdaterStoppnavn(Stopp innStopp)
        {
            if (innStopp == null)
            {
                _log.LogInformation("Det er ikke noe stoppobjekt å endre navn på!");
                return false;
            }
            Stopp stopp = await _db.Stopp.FindAsync(innStopp.Id);
            stopp.Navn = innStopp.Navn;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}