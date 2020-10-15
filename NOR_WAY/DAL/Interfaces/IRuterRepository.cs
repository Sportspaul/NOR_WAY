using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IRuterRepository
    {
        Task<bool> NyRute(Ruter rute);

        Task<List<Ruter>> HentAlleRuter();

        Task<List<RuteMedStopp>> HentRuterMedStopp();

        Task<Ruter> HentEnRute(string linjekode);

        Task<bool> OppdaterRute(Ruter endretRute);

        Task<bool> FjernRute(string linjekode);
    }
}
