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
        public Task<bool> FjernRuteStopp(int stoppNummer, string linjekode)
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

        public Task<bool> OppdaterRuteStopp(int stoppNumer, string linjekode, RuteStoppModel oppdatertRuteStopp)
        {
            throw new NotImplementedException();
        }
    }
}
