using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    public class AdminInnloggingRepository : IAdminInnloggingRepository
    {
        public Task<bool> LoggInn(string brukernavn, string passord)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoggUt()
        {
            throw new NotImplementedException();
        }

        //TODO: Midlertidig til vi vet om denne skal ligge her
        public Task<bool> NyAdmin(string brukernavn, string passord)
        {
            throw new NotImplementedException();
        }
    }
}
