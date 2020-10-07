using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminRuterController
    {
        private readonly IAdminAvgangRepository _db;
        private ILogger<AdminRuterController> _log;

        public AdminRuterController(IAdminAvgangRepository db, ILogger<AdminRuterController> log)
        {
            _db = db;
            _log = log;
        }

        public Task<ActionResult> FjernRute(string linjekode)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> HentAlleRuter()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> NyRute()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> OppdaterRute(Ruter rute, string linjekode)
        {
            throw new NotImplementedException();
        }
    }
}
