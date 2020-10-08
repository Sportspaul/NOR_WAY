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
    public class RuterController : ControllerBase
    {
        private readonly IRuterRepository _db;
        private ILogger<RuterController> _log;

        public RuterController(IRuterRepository db, ILogger<RuterController> log)
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

        public async Task<ActionResult> HentRuterMedStopp()
        {
            List<RuteMedStopp> rutene = await _db.HentRuterMedStopp();
            if (rutene == null)
            {
                _log.LogInformation("Rutene ble ikke funnet");
                return NotFound("Rutene ble ikke funnet");
            }
            return Ok(rutene);
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
