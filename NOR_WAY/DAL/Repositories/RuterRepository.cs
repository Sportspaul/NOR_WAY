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
    public class RuterRepository : IRuterRepository
    {
        private readonly BussContext _db;
        private readonly ILogger<RuterRepository> _log;

        public RuterRepository(BussContext db, ILogger<RuterRepository> log)
        {
            _db = db;
            _log = log;
        }

        public Task<bool> FjernRute(string linjekode)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ruter>> HentAlleRuter()
        {
            throw new NotImplementedException();
        }

        public async Task<List<RuteMedStopp>> HentRuterMedStopp()
        {
            try
            {
                List<RuteMedStopp> RuteDataene = new List<RuteMedStopp>();

                // Henter alle rutene fra DB
                List<Ruter> AlleRutene = await _db.Ruter.Select(r => new Ruter
                {
                    Linjekode = r.Linjekode,
                    Rutenavn = r.Rutenavn,
                    TilleggPerStopp = r.TilleggPerStopp,
                    Startpris = r.Startpris
                }).ToListAsync();

                // Lopper gjennom alle rutene i DB
                foreach (Ruter rute in AlleRutene)
                {
                    RuteMedStopp rutedata = new RuteMedStopp
                    {
                        Stoppene = new List<string>(),
                        MinutterTilNesteStopp = new List<int>(),
                        Rutenavn = rute.Rutenavn,
                        Linjekode = rute.Linjekode,
                        TilleggPerStopp = rute.TilleggPerStopp,
                        Startpris = rute.Startpris
                    };

                    // Henter alle ruteStopp som hører til spesifikk rute
                    List<RuteStopp> ruteStopp = await _db.RuteStopp
                        .Where(rs => rs.Rute.Linjekode == rute.Linjekode)
                        .OrderBy(rs => rs.StoppNummer)
                        .Select(rs => new RuteStopp
                        {
                            MinutterTilNesteStopp = rs.MinutterTilNesteStopp,
                            Rute = rs.Rute,
                            Stopp = rs.Stopp
                        }).ToListAsync();

                    // Looper gjennom alle ruteStoppp og legger navn og tid i lister
                    foreach (RuteStopp rutestopp in ruteStopp)
                    {
                        rutedata.Stoppene.Add(rutestopp.Stopp.Navn);
                        rutedata.MinutterTilNesteStopp.Add(rutestopp.MinutterTilNesteStopp);
                    }
                    RuteDataene.Add(rutedata); // Legger objektet i listen
                }

                return RuteDataene;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public Task<bool> NyRute(Ruter nyRute, List<RuteStoppModel> nyeRuteStopp)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterRute(string linjekode, Ruter rute)
        {
            throw new NotImplementedException();
        }
    }
}
