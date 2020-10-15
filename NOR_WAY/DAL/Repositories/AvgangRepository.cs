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
    public class AvgangRepository : IAvgangRepository
    {
        private readonly BussContext _db;
        private readonly ILogger<AvgangRepository> _log;
        private readonly HjelpeRepository _hjelp;

        public AvgangRepository(BussContext db, ILogger<AvgangRepository> log)
        {
            _hjelp = new HjelpeRepository(db, log);
            _db = db;
            _log = log;
        }

        public async Task<Reisedetaljer> FinnNesteAvgang(Avgangkriterier input)
        {
            // Henter avgang- og påstigningsstoppet fra DB
            Stopp startStopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == input.StartStopp);
            Stopp sluttStopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == input.SluttStopp);
            if (startStopp.Equals(sluttStopp))
            {
                _log.LogInformation("Sluttstopp kan ikke være lik startstopp!");
                return null;
            }

            // Finner alle Rutene som inkluderer påstigning og som inkluderer avstigning
            List<Ruter> startStoppRuter = await _hjelp.FinnRuteneTilStopp(startStopp);
            List<Ruter> sluttStoppRuter = await _hjelp.FinnRuteneTilStopp(sluttStopp);

            // Finner ruten påstigning og avstigning har til felles
            Ruter fellesRute = _hjelp.FinnFellesRute(startStoppRuter, sluttStoppRuter);
            if (fellesRute == null) { return null; } // Hvis stoppene ikke har noen felles ruter

            // Finne ut hvilket stoppNummer påstigning og avstigning har i den felles ruten
            int stoppNummer1 = await _hjelp.FinnStoppNummer(startStopp, fellesRute);
            int stoppNummer2 = await _hjelp.FinnStoppNummer(sluttStopp, fellesRute);
            // Hvis første stopp kommer senere i ruten enn siste stopp
            if (stoppNummer1 > stoppNummer2)
            {
                _log.LogInformation("Seneste stopp har ikke lavere stoppnummer enn tidligste stopp!");
                return null;
            }

            int reisetid = await _hjelp.BeregnReisetid(stoppNummer1, stoppNummer2, fellesRute); // Beregner reisetiden fra stopp påstigning til avstigning
            int antallBilletter = input.Billettyper.Count();    // antall billetter brukeren ønsker
            DateTime innTid = _hjelp.StringTilDateTime(input.Dato, input.Tidspunkt);   // Konverterer fra strings til DateTime

            // Finne neste avgang som passer, basert på brukerens input
            Avganger nesteAvgang = await _hjelp.NesteAvgang(fellesRute, reisetid, input.AvreiseEtter, innTid, antallBilletter);
            if (nesteAvgang == null) { return null; }  // Hvis ingen avgang ble funnet

            // Beregner avreise og ankomst
            DateTime avreise = await _hjelp.BeregnAvreisetid(nesteAvgang.Avreise, stoppNummer1, fellesRute);
            DateTime ankomst = avreise.AddMinutes(reisetid);

            // Konverterer avreise og ankomst fra DateTime til en strings
            string utAvreise = avreise.ToString("dd-MM-yyyy HH:mm");
            string utAnkomst = ankomst.ToString("dd-MM-yyyy HH:mm");

            // Beregner prisen basert på startpris og antall stopp
            int antallStopp = stoppNummer2 - stoppNummer1;
            int pris = await _hjelp.BeregnPris(fellesRute, antallStopp, input.Billettyper);

            // Opretter Avgang-objektet som skal sendes til klienten
            Reisedetaljer utAvgang = new Reisedetaljer
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

        public async Task<bool> FjernAvgang(int id)
        {
            try
            {
                Avganger avgang = await _db.Avganger.FindAsync(id); // Avgangen som skal slettes

                // Sletter alle Ordre og Ordrelinjer som tilhører Avgangen fra DB
                List<Ordre> ordre = await _db.Ordre.Where(o => o.Avgang == avgang).ToListAsync();
                foreach (Ordre o in ordre)
                {
                    List<Ordrelinjer> ordrelinjer = await _db.Ordrelinjer.Where(ol => ol.Ordre == o).ToListAsync();
                    foreach (Ordrelinjer ol in ordrelinjer)
                    {
                        _db.Ordrelinjer.Remove(ol);
                    }
                    _db.Ordre.Remove(o);
                }

                _db.Avganger.Remove(avgang);    // Sletter Avganger-objekter fra DB
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
            
        }

        public async Task<List<AvgangModel>> HentAvganger(string linjekode, int sidenummer)
        {
            try
            {
                // TODO: Kan justere til et annet tall enn 20 for å se best mulig ut på frontend
                int hoppOver = sidenummer * 10;   // Antall elementer som skal hoppes over
                // Sorterer etter Avreise, hopper over (sidenummer * 20) elementer og henter 20 elementer
                List<Avganger> avganger = await _db.Avganger.Where(a => a.Rute.Linjekode == linjekode)
                    .OrderBy(a => a.Avreise).Skip(hoppOver).Take(10).ToListAsync();

                // Konverter hvert Avganger-objekt til AvagnModel og legger de i en liste
                List<AvgangModel> utAvganger = new List<AvgangModel>();
                foreach (Avganger a in avganger)
                {
                    utAvganger.Add(
                        new AvgangModel
                        {
                            Id = a.Id,
                            Avreise = a.Avreise.ToString("dd-MM-yyyy HH:mm"),
                            SolgteBilletter = a.SolgteBilletter
                        }
                    );
                }

                return utAvganger;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<bool> NyAvgang(NyAvgang nyAvgang)
        {
            try
            {
                Ruter rute = await _db.Ruter.FindAsync(nyAvgang.Linjekode);
                Avganger avgang = new Avganger
                {
                    Avreise = _hjelp.StringTilDateTime(nyAvgang.Dato, nyAvgang.Tidspunkt),
                    SolgteBilletter = nyAvgang.SolgteBilletter,
                    Rute = rute
                };
                _db.Avganger.Add(avgang);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }


        }

        public async Task<bool> OppdaterAvgang(Avreisetid nyAvreisetid)
        {
            try
            {
                Avganger gammelAvgang = await _db.Avganger.FindAsync(nyAvreisetid.Id);
                DateTime nyAvreise = _hjelp.StringTilDateTime(nyAvreisetid.Dato, nyAvreisetid.Tidspunkt);
                gammelAvgang.Avreise = nyAvreise;
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
