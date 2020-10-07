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
        private ILogger<BestillReiseRepository> _log;


        public RuterRepository(BussContext db, ILogger<BestillReiseRepository> log)
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

        public async Task<List<RuteData>> HentRuterMedStopp()
        {
            try
            {
                List<RuteData> RuteDataene = new List<RuteData>();

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
                    RuteData rutedata = new RuteData
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

        public Task<bool> NyRute()
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterRute(Ruter rute, string linjekode)
        {
            throw new NotImplementedException();
        }
    }
}
