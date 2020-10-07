using System.Collections.Generic;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IBestillReiseRepository
    {
        Task<Avgang> FinnNesteAvgang(AvgangParam param);

        Task<bool> FullforOrdre(KundeOrdre ordre);
    }
}