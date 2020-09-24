using System.Collections.Generic;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL
{
    public interface IBussRepository
    {
        Task<List<Stopp>> HentAlleStopp();

        Task<List<Stopp>> FinnMuligeStartStopp(InnStopp startStopp);

        Task<List<Stopp>> FinnMuligeSluttStopp(InnStopp sluttStopp);

        Task<List<Billettyper>> HentAlleBillettyper();

        Task<List<RuteData>> HentAlleRuter();

        Task<Avgang> FinnNesteAvgang(AvgangParam param);

        Task<bool> FullforOrdre(KundeOrdre ordre);
    }
}