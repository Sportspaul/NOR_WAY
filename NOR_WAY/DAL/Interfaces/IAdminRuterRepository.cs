using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IAdminRuterRepository
    {
        Task<bool> NyRute();

        Task<List<Ruter>> HentAlleRuter();

        Task<bool> OppdaterRute(Ruter rute, string linjekode);

        Task<bool> FjernRute(string linjekode);
    }
}
