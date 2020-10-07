using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    public class BillettyperRepository : IBillettyperRepository
    {
        public Task<bool> NyBillettType()
        {
            throw new NotImplementedException();
        }

        public Task<List<Billettyper>> HentAlleBillettyper()
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterBillettType(string navn)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FjernBillettType(string navn)
        {
            throw new NotImplementedException();
        }
}
}
