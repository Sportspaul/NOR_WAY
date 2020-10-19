using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    [ExcludeFromCodeCoverage]
    public class OrdreRepository : IOrdreRepository
    {
        private readonly BussContext _db;
        private readonly ILogger<OrdreRepository> _log;
        private readonly HjelpeRepository _hjelp;

        public OrdreRepository(BussContext db, ILogger<OrdreRepository> log)
        {
            _hjelp = new HjelpeRepository(db, log);
            _db = db;
            _log = log;
        }

        // Fullfør ordre
        public async Task<bool> FullforOrdre(NyOrdre ordreModel)
        {
            try
            {
                // Henter ut ruten som tilhører OrdreModel
                Ruter rute = await _db.Ruter.FirstOrDefaultAsync(r => r.Linjekode == ordreModel.Linjekode);

                // Henter Avgangens Id
                Avganger avgang = await _db.Avganger.FirstOrDefaultAsync(a => a.Id == ordreModel.AvgangId);

                // Finner startStopp, og finner stoppnummeret i ruten
                Stopp startStopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == ordreModel.StartStopp);
                int stoppNummer1 = await _hjelp.FinnStoppNummer(startStopp, rute);

                // Finner sluttStopp, og finner stoppnummeret i ruten
                Stopp sluttStopp = await _db.Stopp.FirstOrDefaultAsync(s => s.Navn == ordreModel.SluttStopp);
                int stoppNummer2 = await _hjelp.FinnStoppNummer(sluttStopp, rute);

                // Regner ut antall stopp
                int antallStopp = stoppNummer2 - stoppNummer1;

                // Finner summen for reisen
                // antallStopp, rute, liste med billettype
                int sum = await _hjelp.BeregnPris(rute, antallStopp, ordreModel.Billettyper);

                // Lager en ordre basert på ordreModel, rute og avgang
                var ordre = new Ordre
                {
                    Epost = ordreModel.Epost,
                    StartStopp = startStopp,
                    SluttStopp = sluttStopp,
                    Sum = sum,
                    Rute = rute,
                    Avgang = avgang
                };

                // Legger ordren til i databasen
                _db.Ordre.Add(ordre);

                // Raden til spesifisert avgang
                Avganger dbAvgang = _db.Avganger.Find(avgang.Id);

                // Går gjennom listen med billettyper
                foreach (string billettype in ordreModel.Billettyper)
                {
                    // Henter ut en billettype i listen
                    Billettyper billettypeObjekt = await _db.Billettyper.FirstOrDefaultAsync(a => a.Billettype == billettype);

                    // Lager en ordrelinje
                    var ordrelinje = new Ordrelinjer
                    {
                        Billettype = billettypeObjekt,
                        Ordre = ordre
                    };

                    // Legger denne ordrelinjen til databasen
                    _db.Ordrelinjer.Add(ordrelinje);

                    // Øker antalll solgte billetter med 1
                    dbAvgang.SolgteBilletter++;
                }

                // Lagrer alt som er blitt lagt til i databasen
                _db.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        public Task<List<OrdreModel>> HentOrdre(string epost)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SlettOrdre(int id)
        {
            throw new NotImplementedException();
        }
    }
}