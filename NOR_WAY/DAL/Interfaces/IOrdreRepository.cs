using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IOrdreRepository
    {
        Task<List<OrdreModel>> HentOrdre(string epost);

        Task<bool> SlettOrdre(int id);
    }
}
