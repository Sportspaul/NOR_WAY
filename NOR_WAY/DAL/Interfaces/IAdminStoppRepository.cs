using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IAdminStoppRepository
    {
        Task<List<StoppInfo>> HentAlleStopp();

        Task<bool> EndreStoppnavn(int Id);
    }
}
