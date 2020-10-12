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
    public class RuteStoppRepository : IRuteStoppRepository
    {
        private readonly BussContext _db;
        private readonly ILogger<RuteStoppRepository> _log;

        public RuteStoppRepository(BussContext db, ILogger<RuteStoppRepository> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<bool> FjernRuteStopp(int stoppNummer, string linjekode)
        {
            try
            {
                // Fjerner aktuelt RuteStopp
                RuteStopp ruteStoppTilFjerning = await _db.RuteStopp
                    .SingleOrDefaultAsync(rs => rs.StoppNummer == stoppNummer && rs.Rute.Linjekode == linjekode);
                _db.Remove(ruteStoppTilFjerning);

                // Henter alle RuteStopp fra samme rute som har likt eller høyre stoppnummer enn det som ble fjernet
                List<RuteStopp> endreStoppNummer = await LiktEllerSenereStoppNummer(stoppNummer, linjekode);

                // Subtraherer alle med stoppnummer som er større eller lik det fjernede rutestoppet med 1
                foreach (RuteStopp rs in endreStoppNummer)
                {
                    rs.StoppNummer--;
                }
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
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
                List<RuteStopp> endreStoppNummer = await LiktEllerSenereStoppNummer(innRuteStopp.StoppNummer, innRuteStopp.Linjekode);

                // Adderer alle med stoppnummer som er større eller lik det nye rutestoppet med 1
                foreach (RuteStopp rs in endreStoppNummer)
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

        // Metode for å oppdatere verdeiene i et RuteStopp
        public async Task<bool> OppdaterRuteStopp(RuteStoppOppdatert oppdatertRuteStopp)
        {
            try
            {
                // Returnerer false hvis nyttStoppNummer er mindre enn det minste eller større en det største som allerede eksisterer
                int antallRuteStopp = await _db.RuteStopp.Where(rs => rs.Rute.Linjekode == oppdatertRuteStopp.Linjekode).CountAsync();
                int nyttStoppNummer = oppdatertRuteStopp.NyttStoppNummer;
                if (nyttStoppNummer > antallRuteStopp || nyttStoppNummer <= 0) { return false; }

                // Fjerner RuteStopp-objektet som skal endres
                bool slettOk = await FjernRuteStopp(oppdatertRuteStopp.GammeltStoppNummer, oppdatertRuteStopp.Linjekode);
                RuteStoppModel ruteStoppModel = new RuteStoppModel {
                    StoppNummer = oppdatertRuteStopp.NyttStoppNummer,
                    MinutterTilNesteStopp = oppdatertRuteStopp.MinutterTilNesteStopp,
                    Stoppnavn = oppdatertRuteStopp.Stoppnavn,
                    Linjekode = oppdatertRuteStopp.Linjekode
                };
                bool nyOk = await NyRuteStopp(ruteStoppModel);  // Legger til et nytt RuteStopp
                if (slettOk && nyOk) {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        // Henter alle RuteStopp fra samme rute som har likt eller høyre stoppnummer
        private async Task<List<RuteStopp>> LiktEllerSenereStoppNummer(int stoppNummer, string linjekode)
        {
            return await _db.RuteStopp
                .Where(rs => rs.StoppNummer >= stoppNummer && rs.Rute.Linjekode == linjekode).ToListAsync();
        }
    }
}
