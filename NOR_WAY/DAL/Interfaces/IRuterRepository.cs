using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IRuterRepository
    {
        Task<bool> NyRute(Ruter nyRute, List<RuteStoppModel> nyeRuteStopp);

        Task<List<Ruter>> HentAlleRuter();

        Task<List<RuteMedStopp>> HentRuterMedStopp();

        Task<bool> OppdaterRute(string linjekode, Ruter endretRute);

        Task<bool> FjernRute(string linjekode);
    }
}
