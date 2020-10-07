using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
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

        public static byte[] LagHash(string passord, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: passord,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
                                numBytesRequested: 32);
        }

        public static byte[] LagSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }
    }
}
