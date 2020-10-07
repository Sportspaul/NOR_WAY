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
        private ILogger<StoppRepository> _log;

        public StoppRepository(BussContext db, ILogger<StoppRepository> log)
        {
            _db = db;
            _log = log;
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

        public Task<List<StoppMedRuter>> HentAlleStoppMedRuter()
        {
            throw new NotImplementedException();
        }

        public Task<bool> EndreStoppnavn(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
