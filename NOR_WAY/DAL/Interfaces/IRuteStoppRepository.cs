using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IRuteStoppRepository
    {

        Task<bool> NyRuteStopp(RuteStoppModel innRuteStopp);

        Task<List<RuteStoppModel>> HentRuteStopp(string linjekode);

        Task<bool> OppdaterRuteStopp(RuteStoppOppdatert ruteStoppOppdatert);

        Task<List<RuteStoppModel>> HentAlleRuteStopp();

        Task<bool> FjernRuteStopp(int stoppNummer, string linjekode);
    }
}
