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
    public class RuterController
    {
        private readonly IAvgangRepository _db;
        private ILogger<RuterController> _log;

        public RuterController(IAvgangRepository db, ILogger<RuterController> log)
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
