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
        private ILogger<AvgangRepository> _log;
        private HjelpeRepository Hjelp;

        public AvgangRepository(BussContext db, ILogger<AvgangRepository> log)
        {
            Hjelp = new HjelpeRepository(db, log);
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
            List<Ruter> startStoppRuter = await Hjelp.FinnRuter(startStopp);
            List<Ruter> sluttStoppRuter = await Hjelp.FinnRuter(sluttStopp);

            // Finner ruten påstigning og avstigning har til felles
            Ruter fellesRute = Hjelp.FinnFellesRute(startStoppRuter, sluttStoppRuter);
            if (fellesRute == null) { return null; } // Hvis stoppene ikke har noen felles ruter

            // Finne ut hvilket stoppNummer påstigning og avstigning har i den felles ruten
            int stoppNummer1 = await Hjelp.FinnStoppNummer(startStopp, fellesRute);
            int stoppNummer2 = await Hjelp.FinnStoppNummer(sluttStopp, fellesRute);
            // Hvis første stopp kommer senere i ruten enn siste stopp
            if (stoppNummer1 > stoppNummer2)
            {
                _log.LogInformation("Seneste stopp har ikke lavere stoppnummer enn tidligste stopp!");
                return null;
            }

            int reisetid = await Hjelp.BeregnReisetid(stoppNummer1, stoppNummer2, fellesRute); // Beregner reisetiden fra stopp påstigning til avstigning
            int antallBilletter = input.Billettyper.Count();    // antall billetter brukeren ønsker
            DateTime innTid = Hjelp.StringTilDateTime(input.Dato, input.Tidspunkt);   // Konverterer fra strings til DateTime

            // Finne neste avgang som passer, basert på brukerens input
            Avganger nesteAvgang = await Hjelp.NesteAvgang(fellesRute, reisetid, input.AvreiseEtter, innTid, antallBilletter);
            if (nesteAvgang == null) { return null; }  // Hvis ingen avgang ble funnet

            // Beregner avreise og ankomst
            DateTime avreise = await Hjelp.BeregnAvreisetid(nesteAvgang.Avreise, stoppNummer1, fellesRute);
            DateTime ankomst = avreise.AddMinutes(reisetid);

            // Konverterer avreise og ankomst fra DateTime til en strings
            string utAvreise = avreise.ToString("dd-MM-yyyy HH:mm");
            string utAnkomst = ankomst.ToString("dd-MM-yyyy HH:mm");

            // Beregner prisen basert på startpris og antall stopp
            int antallStopp = stoppNummer2 - stoppNummer1;
            int pris = await Hjelp.BeregnPris(fellesRute, antallStopp, input.Billettyper);

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

        public Task<bool> FjernAvgang(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Avganger> HentAvganger(string linjekode, int side)
        {
            throw new NotImplementedException();
        }

        public Task<bool> NyAvgang(AvgangModel nyAvgang)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterAvgang(int Id, Avganger oppdaterAvgang)
        {
            throw new NotImplementedException();
        }
    }
}
