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
    public class AdminRuterStoppController
    {
        private readonly IAdminAvgangRepository _db;
        private ILogger<AdminRuterStoppController> _log;

        public AdminRuterStoppController(IAdminAvgangRepository db, ILogger<AdminRuterStoppController> log)
        {
            _db = db;
            _log = log;
        }

        public Task<ActionResult> FjernRuteStopp(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> HentAlleRuteStopp()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> HentRuteStopp(string linjekode)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> NyRuteStopp(NyRuteStopp nyRuteStopp)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> OppdaterStoppNavn(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
