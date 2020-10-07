using System.Collections.Generic;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IBestillReiseRepository
    {
        Task<List<Stopp>> FinnMuligeStartStopp(InnStopp startStopp);

        Task<List<Stopp>> FinnMuligeSluttStopp(InnStopp sluttStopp);

        Task<Avgang> FinnNesteAvgang(AvgangParam param);

        Task<bool> FullforOrdre(KundeOrdre ordre);
    }
}