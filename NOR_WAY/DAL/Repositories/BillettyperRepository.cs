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
                    Id = b.Id,
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

        public async Task<bool> OppdaterBillettype(Billettyper opdatertBillettype)
        {
            try
            {
                Billettyper billettype = await _db.Billettyper.FindAsync(opdatertBillettype.Id);
                billettype.Billettype = opdatertBillettype.Billettype;
                billettype.Rabattsats = opdatertBillettype.Rabattsats;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<Billettyper> HentEnBillettype(int id)
        {
            try
            {
                return await _db.Billettyper.FindAsync(id);
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return null;
            }
        }
    }
}
