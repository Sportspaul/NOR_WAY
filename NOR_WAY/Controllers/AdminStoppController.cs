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
    public class AdminStoppController
    {
        private readonly IAdminAvgangRepository _db;
        private ILogger<AdminStoppController> _log;

        public AdminStoppController(IAdminAvgangRepository db, ILogger<AdminStoppController> log)
        {
            _db = db;
            _log = log;
        }

        public Task<ActionResult> HentAlleStopp()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> EndreStoppnavn(int Id)
        {
            throw new NotImplementedException();
        }
    }
}
