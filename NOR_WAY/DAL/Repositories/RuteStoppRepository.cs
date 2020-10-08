using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    public class RuteStoppRepository : IRuteStoppRepository
    {
        public Task<bool> FjernRuteStopp(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<List<RuteStoppModel>> HentAlleRuteStopp()
        {
            throw new NotImplementedException();
        }

        public Task<RuteStopp> HentRuteStopp(string linjekode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> NyRuteStopp(RuteStoppModel nyRuteStopp)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterStoppNavn(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
