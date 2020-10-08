using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    public class OrdreRepository : IOrdreRepository
    {
        public Task<List<OrdreModel>> HentOrdre(string epost)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SlettOrdre(int id)
        {
            throw new NotImplementedException();
        }
    }
}
