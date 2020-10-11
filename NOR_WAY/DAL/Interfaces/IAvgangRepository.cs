﻿using System;
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

        Task<List<Avganger>> HentAvganger(string linjekode, int sidenummer);

        Task<bool> OppdaterAvgang(int id, Avganger oppdaterAvgang);

        Task<bool> FjernAvgang(int id);
    }
}
