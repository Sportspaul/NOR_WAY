using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    interface IAdminAvgangRepository
    {

        Task<bool> NyAvgang(NyAvgang nyAvgang);

        Task<Avganger> HentAvganger(string linjekode, int side);

        Task<bool> OppdaterAvgang(Avgang avgang, int Id);

        Task<bool> FjernAvgang(int Id);
    }
}
