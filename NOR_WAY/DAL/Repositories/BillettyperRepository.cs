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
        private readonly ILogger<BillettyperRepository> _log;

        public BillettyperRepository(BussContext db, ILogger<BillettyperRepository> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<bool> NyBillettype(Billettyper innBillettype)
        {
            try
            {
                Billettyper nyBillettype = new Billettyper
                {
                    Billettype = innBillettype.Billettype,
                    Rabattsats = innBillettype.Rabattsats
                };
                _db.Billettyper.Add(nyBillettype);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
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

        public async Task<bool> OppdaterRabattsats(Billettyper oppdatertRabattsats)
        {
            try
            {
                Billettyper billettype = await _db.Billettyper
                    .SingleOrDefaultAsync(bt => bt.Billettype == oppdatertRabattsats.Billettype);
                billettype.Rabattsats = oppdatertRabattsats.Rabattsats;
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
