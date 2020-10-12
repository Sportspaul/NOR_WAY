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

        public Task<bool> FjernRuteStopp(int stoppNummer, string linjekode)
        {
            throw new NotImplementedException();
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

        public Task<bool> NyRuteStopp(RuteStoppModel nyRuteStopp)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterRuteStopp(int stoppNumer, string linjekode, RuteStoppModel oppdatertRuteStopp)
        {
            throw new NotImplementedException();
        }
    }
}
