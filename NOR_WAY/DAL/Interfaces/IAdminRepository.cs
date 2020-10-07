using NOR_WAY.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IAdminRepository
    {
        Task<bool> LoggInn(string brukernavn, string passord);

        Task<bool> LoggUt();

        Task<bool> NyAdmin(string brukernavn, string passord);
    }
}
