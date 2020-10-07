using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IAdminOrdreRepository
    {
        Task<List<KundeOrdre>> HentOrdre(string epost);

        Task<bool> SlettOrdre(int id);
    }
}
