﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IStoppRepository
    {
        Task<List<Stopp>> FinnMuligeStartStopp(StoppModel sluttStopp);

        Task<List<Stopp>> FinnMuligeSluttStopp(StoppModel startStopp);

        Task<Stopp> HentEtStopp(int id);

        Task<List<Stopp>> HentAlleStopp();

        Task<List<StoppMedLinjekoder>> HentAlleStoppMedRuter();

        Task<bool> OppdaterStoppnavn(Stopp innStopp);
    }
}