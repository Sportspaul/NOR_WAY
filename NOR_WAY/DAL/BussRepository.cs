using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NOR_WAY.Model;

namespace NOR_WAY.DAL
{
    public class BussRepository : IBussRepository
    {
        private readonly BussContext _db;

        public BussRepository(BussContext db)
        {
            _db = db;
        }

        public async Task<Avgang> FinnNesteAvgang(AvgangParam param)
        {
            throw new NotImplementedException();
        }

        
        public async Task<bool> FullforOrdre(KundeOrdre ordre)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Billettyper>> HentAlleBillettyper()
        {
            List<Billettyper> alleBillettyper = await _db.Billettyper.Select(b => new Billettyper
            {
                Billettype = b.Billettype,
                Rabattsats = b.Rabattsats
            }).ToListAsync();

            return alleBillettyper;
        }

        public async Task<List<Stopp>> HentAlleStopp()
        {
            List<Stopp> alleStopp = await _db.Stopp.Select(s => new Stopp
            {
                Navn = s.Navn
            }).ToListAsync();

            return alleStopp;
        }
    }
}
