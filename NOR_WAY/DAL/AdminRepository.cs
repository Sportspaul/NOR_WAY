using System;
using Microsoft.Extensions.Logging;

namespace NOR_WAY.DAL
{
    public class AdminRepository : IAdminRepository
    {
        private readonly BussContext _db;

        private ILogger<AdminRepository> _log;

        public AdminRepository(BussContext db, ILogger<AdminRepository> log)
        {
            _db = db;
            _log = log;
        }
    }
}
