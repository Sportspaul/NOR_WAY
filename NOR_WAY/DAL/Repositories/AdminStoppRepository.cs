using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    public class AdminStoppRepository : IAdminStoppRepository
    {
        public Task<List<StoppInfo>> HentAlleStopp()
        {
            throw new NotImplementedException();
        }

        public Task<bool> EndreStoppnavn(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
