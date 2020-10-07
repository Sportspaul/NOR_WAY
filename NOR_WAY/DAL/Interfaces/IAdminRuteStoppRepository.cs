using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    interface IAdminRuteStoppRepository
    {

        Task<bool> NyRuteStopp(NyRuteStopp nyRuteStopp);

        Task<RuteStopp> HentRuteStopp(string linjekode);

        Task<bool> OppdaterStoppNavn(int Id);

        Task<List<NyRuteStopp>> HentAlleRuteStopp();

        Task<bool> FjernRuteStopp(int Id);
    }
}
