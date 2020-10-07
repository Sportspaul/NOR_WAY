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
    public class AdminInnloggingController
    {
        private readonly IAdminAvgangRepository _db;
        private ILogger<AdminInnloggingController> _log;

        public AdminInnloggingController(IAdminAvgangRepository db, ILogger<AdminInnloggingController> log)
        {
            _db = db;
            _log = log;
        }

        public Task<ActionResult> LoggInn(string brukernavn, string passord)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> LoggUt()
        {
            throw new NotImplementedException();
        }

        //TODO: Midlertidig til vi vet om denne skal ligge her
        public Task<ActionResult> NyAdmin(string brukernavn, string passord)
        {
            throw new NotImplementedException();
        }
    }
}
