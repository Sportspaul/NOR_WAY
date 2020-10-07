using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IRuterRepository
    {
        Task<bool> NyRute();

        Task<List<Ruter>> HentAlleRuter();

        Task<List<RuteData>> HentRuterMedStopp();

        Task<bool> OppdaterRute(Ruter rute, string linjekode);

        Task<bool> FjernRute(string linjekode);
    }
}
