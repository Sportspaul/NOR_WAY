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

        public async Task<bool> FjernRute(string linjekode)
        {
            try
            {
                // Heter Ruter-objektet og tilhørende RuteStopp, Ordre og Avganger fra DB
                Ruter rute = await _db.Ruter.FindAsync(linjekode);
                List<RuteStopp> ruteStopp = await _db.RuteStopp.Where(rs => rs.Rute == rute).ToListAsync();
                List<Ordre> ordre = await _db.Ordre.Where(o => o.Rute == rute).ToListAsync();
                List<Avganger> avganger = await _db.Avganger.Where(a => a.Rute == rute).ToListAsync();

                // Fjerner alle tilhørende rutestopp fra DB
                foreach (var rs in ruteStopp)
                {
                    _db.RuteStopp.Remove(rs);
                }

                // Fjerner alle tilhørende ordre og ordrelinjer fra DB
                foreach (var o in ordre)
                {
                    List<Ordrelinjer> ordrelinjer = await _db.Ordrelinjer.Where(ol => ol.Ordre == o).ToListAsync();
                    foreach (Ordrelinjer ol in ordrelinjer)
                    {
                        _db.Ordrelinjer.Remove(ol);
                    }
                    _db.Ordre.Remove(o);
                }

                // Fjerner alle tilhørende avganer fra DB
                foreach (var a in avganger)
                {
                    _db.Avganger.Remove(a);
                }

                // Fjerner ruten, lagrer endringen og returnere true hvis alt gikk fint
                _db.Ruter.Remove(rute);
                _db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<List<Ruter>> HentAlleRuter()
        {
            try
            {
                // Henter alle rutene fra DB
                List<Ruter> alleRutene = await _db.Ruter.Select(r => new Ruter
                {
                    Linjekode = r.Linjekode,
                    Rutenavn = r.Rutenavn,
                    TilleggPerStopp = r.TilleggPerStopp,
                    Startpris = r.Startpris,
                    Kapasitet = r.Kapasitet
                }).ToListAsync();

                return alleRutene;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<Ruter> HentEnRute(string linjekode)
        {
            try
            {
                Ruter rute = await _db.Ruter.FindAsync(linjekode);
                return rute;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<List<RuteMedStopp>> HentRuterMedStopp()
        {
            try
            {
                List<RuteMedStopp> RuteDataene = new List<RuteMedStopp>();

                // Henter alle rutene fra DB
                List<Ruter> AlleRutene = await HentAlleRuter();

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

        // Metode for å legge til en ny rute i DB
        public async Task<bool> NyRute(Ruter rute)
        {
            try
            {
                _db.Ruter.Add(rute);
                await _db.SaveChangesAsync();  // Lagrer endringene
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        // Metode for å oppdaterer rutedetaljer
        public async Task<bool> OppdaterRute(Ruter endretRute)
        {
            try
            {
                // Finner ruten som skal oppdateres og oppdaterer verdiene
                Ruter gammelRute = await _db.Ruter.FindAsync(endretRute.Linjekode);
                gammelRute.Rutenavn = endretRute.Rutenavn;
                gammelRute.Startpris = endretRute.Startpris;
                gammelRute.TilleggPerStopp = endretRute.TilleggPerStopp;
                gammelRute.Kapasitet = endretRute.Kapasitet;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }
    }
}