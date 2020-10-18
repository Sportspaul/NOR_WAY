using System.Collections.Generic;
using System.Threading.Tasks;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IBillettyperRepository
    {
        Task<bool> NyBillettype(Billettyper nyBillettype);

        Task<Billettyper> HentEnBillettype(int id);

        Task<List<Billettyper>> HentAlleBillettyper();

        Task<bool> OppdaterBillettype(Billettyper oppdatertBillettype);
    }
}