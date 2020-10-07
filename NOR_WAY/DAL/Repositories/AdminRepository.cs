using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    [ExcludeFromCodeCoverage]
    public class AdminRepository : IAdminRepository
    {
        private readonly BussContext _db;

        private ILogger<AdminRepository> _log;

        public AdminRepository(BussContext db, ILogger<AdminRepository> log)
        {
            _db = db;
            _log = log;
        }

        public Task<bool> EndreStoppnavn(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FjernAvgang(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FjernBillettType(string navn)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FjernRute(string linjekode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FjernRuteStopp(int Id)
        {
            throw new NotImplementedException();
        }

        // TODO: Fjern Eksempel
        public Task<string> HeiVerden()
        {
            throw new NotImplementedException();
        }

        public Task<List<Billettyper>> HentAlleBillettyper()
        {
            throw new NotImplementedException();
        }

        public Task<List<Ruter>> HentAlleRuter()
        {
            throw new NotImplementedException();
        }

        public Task<List<NyRuteStopp>> HentAlleRuteStopp()
        {
            throw new NotImplementedException();
        }

        public Task<List<StoppInfo>> HentAlleStopp()
        {
            throw new NotImplementedException();
        }

        public Task<Avganger> HentAvganger(string linjekode, int side)
        {
            throw new NotImplementedException();
        }

        public Task<List<KundeOrdre>> HentOrdre(string epost)
        {
            throw new NotImplementedException();
        }

        public Task<RuteStopp> HentRuteStopp(string linjekode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoggInn(string brukernavn, string passord)
        {
            throw new NotImplementedException();
        }

        public Task<bool> LoggUt()
        {
            throw new NotImplementedException();
        }

        public Task<bool> NyAdmin(string brukernavn, string passord)
        {
            throw new NotImplementedException();
        }

        public Task<bool> NyAvgang(NyAvgang nyAvgang)
        {
            throw new NotImplementedException();
        }

        public Task<bool> NyBillettType()
        {
            throw new NotImplementedException();
        }

        public Task<bool> NyRute()
        {
            throw new NotImplementedException();
        }

        public Task<bool> NyRuteStopp(NyRuteStopp nyRuteStopp)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterAvgang(Avgang avgang, int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterBillettType(string navn)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterRute(Ruter rute, string linjekode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> OppdaterStoppNavn(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> SlettOrdre(int id)
        {
            throw new NotImplementedException();
        }
    }
}
