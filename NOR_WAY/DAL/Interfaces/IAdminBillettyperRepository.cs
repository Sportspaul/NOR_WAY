using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IAdminBillettyperRepository
    {
        Task<bool> NyBillettType();

        Task<List<Billettyper>> HentAlleBillettyper();

        Task<bool> OppdaterBillettType(string navn);

        Task<bool> FjernBillettType(string navn);
    }
}
