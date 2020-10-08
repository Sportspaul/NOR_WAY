using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IRuteStoppRepository
    {

        Task<bool> NyRuteStopp(RuteStoppModel nyRuteStopp);

        Task<RuteStopp> HentRuteStopp(string linjekode);

        Task<bool> OppdaterStoppNavn(int Id);

        Task<List<RuteStoppModel>> HentAlleRuteStopp();

        Task<bool> FjernRuteStopp(int Id);
    }
}
