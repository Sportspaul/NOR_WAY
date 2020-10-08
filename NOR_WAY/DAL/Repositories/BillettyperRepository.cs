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
    public class BillettyperRepository : IBillettyperRepository
    {
        private readonly BussContext _db;
        private ILogger<BillettyperRepository> _log;

        public BillettyperRepository(BussContext db, ILogger<BillettyperRepository> log)
        {
            _db = db;
            _log = log;
        }

        public Task<bool> NyBillettType(Billettyper nyBillettype)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Billettyper>> HentAlleBillettyper()
        {
            try
            {
                List<Billettyper> alleBillettyper = await _db.Billettyper.Select(b => new Billettyper
                {
                    Billettype = b.Billettype,
                    Rabattsats = b.Rabattsats
                }).ToListAsync();

                return alleBillettyper;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }

        public Task<bool> OppdaterRabattsats(Billettyper oppdatertRabattsats)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FjernBillettType(string navn)
        {
            throw new NotImplementedException();
        }
}
}
