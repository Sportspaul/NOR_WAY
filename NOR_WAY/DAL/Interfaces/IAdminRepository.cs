using NOR_WAY.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NOR_WAY.DAL.Interfaces
{
    public interface IAdminRepository
    {
        // Ruter: CRUD
        Task<bool> NyRute();

        Task<List<Ruter>> HentAlleRuter();

        Task<bool> OppdaterRute(Ruter rute, string linjekode);

        Task<bool> FjernRute(string linjekode);

        // Avgang: CRUD

        Task<bool> NyAvgang(NyAvgang nyAvgang);

        Task<Avganger> HentAvganger(string linjekode, int side);

        Task<bool> OppdaterAvgang(Avgang avgang, int Id);

        Task<bool> FjernAvgang(int Id);

        // RuteStopp: CRUD

        Task<bool> NyRuteStopp(NyRuteStopp nyRuteStopp);

        Task<RuteStopp> HentRuteStopp(string linjekode);

        Task<bool> OppdaterStoppNavn(int Id);

        Task<List<NyRuteStopp>> HentAlleRuteStopp();

        Task<bool> FjernRuteStopp(int Id);

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
