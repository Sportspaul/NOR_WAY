using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    public class BrukereRepository : IBrukereRepository
    {
        private readonly BussContext _db;
        private readonly ILogger<BrukereRepository> _log;

        public BrukereRepository(BussContext db, ILogger<BrukereRepository> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<bool> LoggInn(BrukerModel bruker)
        {

            try
            {
                // Henter brukeren som matcher input-brukernavnet
                Brukere funnetBruker = await _db.Brukere
                    .FirstOrDefaultAsync(b => b.Brukernavn == bruker.Brukernavn);

                // Sjekker om input-passord + salt matcher passordet + salt i DB 
                byte[] hash = LagHash(bruker.Passord, funnetBruker.Salt);
                bool ok = hash.SequenceEqual(funnetBruker.Passord);
                if (ok)
                {
                    return true;
                }
                return false;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> NyAdmin(BrukerModel bruker)
        {
            try
            {
                byte[] salt = LagSalt();
                byte[] passordHash = LagHash(bruker.Passord, salt);

                Brukere nyAdmin = new Brukere
                {
                    Brukernavn = bruker.Brukernavn,
                    Passord = passordHash,
                    Salt = salt,
                    Tilgang = "Admin"
                };

                _db.Brukere.Add(nyAdmin);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _log.LogInformation(e.Message);
                return false;
            }
        }

        /* Forelesers kode */
        public static byte[] LagHash(string passord, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                                password: passord,
                                salt: salt,
                                prf: KeyDerivationPrf.HMACSHA512,
                                iterationCount: 1000,
                                numBytesRequested: 32);
        }

        /* Forelesers kode */
        public static byte[] LagSalt()
        {
            var csp = new RNGCryptoServiceProvider();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }
    }
}
