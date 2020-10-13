using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IBillettyperRepository
    {
        Task<bool> NyBillettype(Billettyper nyBillettype);

        Task<List<Billettyper>> HentAlleBillettyper();

        Task<bool> OppdaterRabattsats(Billettyper oppdatertRabattsats);

        Task<bool> FjernBillettype(string navn);
    }
}
