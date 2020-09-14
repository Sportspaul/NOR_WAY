using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NOR_WAY.Model;

namespace NOR_WAY.DAL
{
    public interface IBussRepository
    {
        Task<List<Stopp>> HentAlleStopp();
        Task<Billettyper> HentAlleBillettyper();
        Task<List<Avgang>> FinnAktuelleAvganger(AvgangParam param);
        Task<bool> FullforOrdre(KundeOrdre ordre);
    }
}
