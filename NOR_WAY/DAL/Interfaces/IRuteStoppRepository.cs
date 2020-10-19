using System.Collections.Generic;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IRuteStoppRepository
    {
        Task<bool> NyRuteStopp(NyRuteStopp innRuteStopp);

        Task<List<RuteStoppModel>> HentRuteStopp(string linjekode);

        Task<NyRuteStopp> HentEtRuteStopp(int id);

        Task<bool> OppdaterRuteStopp(NyRuteStopp ruteStoppOppdatert);

        Task<bool> FjernRuteStopp(int Id);
    }
}