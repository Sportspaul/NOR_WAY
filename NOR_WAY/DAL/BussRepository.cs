using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<List<Avgang>> FinnAktuelleAvganger(AvgangParam param)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> FullforOrdre(KundeOrdre ordre)
        {
            throw new NotImplementedException();
        }

        public async Task<Billettyper> HentAlleBillettyper()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Stopp>> HentAlleStopp()
        {
            throw new NotImplementedException();
        }
    }
}
