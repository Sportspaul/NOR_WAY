using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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

        public async Task<List<OrdreModel>> HentOrdre(string epost)
        {
            try
            {
                List<Ordre> ordreListe = await _db.Ordre
                    .Where(o => o.Epost == epost)
                    .ToListAsync();

                List<string> billettypeListe = new List<string>();
                List<OrdreModel> ordreModelListe = new List<OrdreModel>();

                foreach(Ordre ordre in ordreListe)
                {
                    billettypeListe = await _db.Ordrelinjer
                        .Where(ol => ol.Ordre == ordre)
                        .Select(ol => ol.Billettype.Billettype)
                        .ToListAsync();

                    OrdreModel ordreModel = new OrdreModel
                    {
                        Id = ordre.Id,
                        Epost = ordre.Epost,
                        StartStopp = ordre.StartStopp.Navn,
                        SluttStopp = ordre.SluttStopp.Navn,
                        Sum = ordre.Sum.ToString(),
                        Linjekode = ordre.Rute.Linjekode,
                        Avreise = ordre.Avgang.Avreise.ToString("dd-MM-yyyy HH:mm"),
                        Billettyper = billettypeListe
                    };
                    ordreModelListe.Add(ordreModel);
                }
                return ordreModelListe;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<bool> SlettOrdre(int id)
        {
            try
            {
                // Finner ordren som skal slettes
                Ordre ordre = await _db.Ordre.FindAsync(id);

                // Finner ordrens tillhørende ordrelinjer og putter dem i en liste
                List<Ordrelinjer> ordrelinjeListe = await _db.Ordrelinjer
                    .Where(o => o.Ordre == ordre)
                    .ToListAsync();

                // Går gjennom listen, og sletter hver enkelt ordelinje
                foreach (Ordrelinjer ordrelinje in ordrelinjeListe)
                {
                    _db.Ordrelinjer.Remove(ordrelinje);
                }

                // Sletter ordren
                _db.Ordre.Remove(ordre);

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