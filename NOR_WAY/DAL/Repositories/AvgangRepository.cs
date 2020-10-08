using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    public class AvgangRepository : IAvgangRepository
    {
        public Task<bool> FjernAvgang(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<Avganger> HentAvganger(string linjekode, int side)
        {
            throw new NotImplementedException();
        }

        public Task<bool> NyAvgang(AvgangModel nyAvgang)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterAvgang(int Id, Avganger oppdaterAvgang)
        {
            throw new NotImplementedException();
        }
    }
}
