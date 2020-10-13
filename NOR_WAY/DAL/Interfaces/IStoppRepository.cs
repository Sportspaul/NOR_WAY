using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IStoppRepository
    {
        Task<List<Stopp>> FinnMuligeStartStopp(StoppModel startStopp);

        Task<List<Stopp>> FinnMuligeSluttStopp(StoppModel sluttStopp);

        Task<List<Stopp>> HentAlleStopp();

        Task<List<StoppMedLinjekoder>> HentAlleStoppMedRuter();

        Task<bool> OppdaterStoppnavn(string id);
    }
}
