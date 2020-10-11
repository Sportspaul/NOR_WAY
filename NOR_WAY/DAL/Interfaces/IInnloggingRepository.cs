using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IInnloggingRepository
    {
        Task<bool> LoggInn(BrukerModel bruker);

        Task<bool> LoggUt();

        //TODO: Midlertidig til vi vet om denne skal ligge her
        Task<bool> NyAdmin(BrukerModel bruker);
    }
}
