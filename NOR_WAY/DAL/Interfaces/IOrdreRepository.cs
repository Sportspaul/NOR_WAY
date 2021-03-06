﻿using System.Collections.Generic;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IOrdreRepository
    {
        Task<bool> FullforOrdre(NyOrdre ordre);

        Task<List<OrdreModel>> HentOrdre(string epost);

        Task<bool> SlettOrdre(int id);
    }
}