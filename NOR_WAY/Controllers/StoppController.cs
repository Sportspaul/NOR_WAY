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
    public class StoppController : ControllerBase
    {
        private readonly IStoppRepository _db;
        private ILogger<StoppController> _log;

        public StoppController(IStoppRepository db, ILogger<StoppController> log)
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
