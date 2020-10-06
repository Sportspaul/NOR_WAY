using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NOR_WAY.DAL
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

        // TODO: Fjern Eksempel
        public Task<string> HeiVerden()
        {
            throw new NotImplementedException();
        }
    }
}
