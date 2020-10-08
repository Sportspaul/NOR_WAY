using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IAvgangRepository
    {
        Task<Reisedetaljer> FinnNesteAvgang(Avgangkriterier kriterier);

        Task<bool> NyAvgang(AvgangModel nyAvgang);

        Task<Avganger> HentAvganger(string linjekode, int side);

        Task<bool> OppdaterAvgang(int Id, Avganger oppdaterAvgang);

        Task<bool> FjernAvgang(int Id);
    }
}
