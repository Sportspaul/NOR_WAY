using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IRuterRepository
    {
        Task<bool> NyRute(RuteModel ruteModel);

        Task<List<Ruter>> HentAlleRuter();

        Task<List<RuteMedStopp>> HentRuterMedStopp();

        Task<bool> OppdaterRute(Ruter endretRute);

        Task<bool> FjernRute(string linjekode);
    }
}
