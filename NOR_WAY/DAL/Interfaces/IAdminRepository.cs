using NOR_WAY.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IAdminRepository
    {
        // Billettyper: CRUD

        Task<bool> NyBillettType();

        Task<List<Billettyper>> HentAlleBillettyper();

        Task<bool> OppdaterBillettType(string navn);

        Task<bool> FjernBillettType(string navn);

        // Stopp: RU

        Task<List<StoppInfo>> HentAlleStopp();

        Task<bool> EndreStoppnavn(int Id);

        // Ordre: RD

        Task<List<KundeOrdre>> HentOrdre(string epost);

        Task<bool> SlettOrdre(int id);

        // Innlogging

        Task<bool> LoggInn(string brukernavn, string passord);

        Task<bool> LoggUt();

        Task<bool> NyAdmin(string brukernavn, string passord);
    }
}
