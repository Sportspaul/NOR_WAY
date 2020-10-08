using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IBillettyperRepository
    {
        Task<bool> NyBillettType(Billettyper nyBillettype);

        Task<List<Billettyper>> HentAlleBillettyper();

        Task<bool> OppdaterRabattsats(Billettyper oppdatertRabattsats);

        Task<bool> FjernBillettType(string navn);
    }
}
