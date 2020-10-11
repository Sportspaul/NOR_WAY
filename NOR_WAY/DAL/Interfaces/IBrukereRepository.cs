using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IBrukereRepository
    {
        Task<bool> LoggInn(BrukerModel bruker);

        Task<bool> LoggUt();

        Task<bool> NyAdmin(BrukerModel bruker);
    }
}
