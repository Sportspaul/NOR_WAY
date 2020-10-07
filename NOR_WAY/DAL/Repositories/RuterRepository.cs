using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.DAL.Interfaces;

namespace NOR_WAY.DAL.Repositories
{
    public class RuterRepository : IRuterRepository
    {
        public Task<bool> FjernRute(string linjekode)
        {
            throw new NotImplementedException();
        }

        public Task<List<Ruter>> HentAlleRuter()
        {
            throw new NotImplementedException();
        }

        public Task<bool> NyRute()
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterRute(Ruter rute, string linjekode)
        {
            throw new NotImplementedException();
        }
    }
}
