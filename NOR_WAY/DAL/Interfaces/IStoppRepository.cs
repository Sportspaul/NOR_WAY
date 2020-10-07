using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IStoppRepository
    {
        Task<List<Stopp>> HentAlleStopp();

        Task<List<StoppMedRuter>> HentAlleStoppMedRuter();

        Task<bool> EndreStoppnavn(int Id);
    }
}
