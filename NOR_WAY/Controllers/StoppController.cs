using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private const string _innlogget = "Innlogget";

        public StoppController(IStoppRepository db, ILogger<StoppController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> FinnMuligeStartStopp(StoppModel startStopp)
        {
            if (ModelState.IsValid)
            {
                List<Stopp> stoppListe = await _db.FinnMuligeStartStopp(startStopp);
                if (stoppListe.Count == 0)
                {
                    _log.LogInformation("Ingen mulige StartStopp ble funnet");
                    return NotFound("Ingen mulige StartStopp ble funnet");
                }
                return Ok(stoppListe); // returnerer alltid OK, null ved tom DB
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalideringen på server");
        }

        public async Task<ActionResult> FinnMuligeSluttStopp(StoppModel sluttStopp)
        {
            if (ModelState.IsValid)
            {
                List<Stopp> stoppListe = await _db.FinnMuligeSluttStopp(sluttStopp);
                if (stoppListe.Count == 0)
                {
                    _log.LogInformation("Ingen mulige SluttStopp ble funnet");
                    return NotFound("Ingen mulige SluttStopp ble funnet");
                }
                return Ok(stoppListe);
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalideringen på server");
        }


        public async Task<ActionResult> HentAlleStopp()
        {
            List<Stopp> alleStopp = await _db.HentAlleStopp();
            return Ok(alleStopp); // returnerer alltid OK, null ved tom DB
        }

        public async Task<ActionResult> OppdaterStoppnavn(Stopp innStopp)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke logget inn");
            }
            if (ModelState.IsValid)
            {
                bool EndringOK = await _db.OppdaterStoppnavn(innStopp);
                if (!EndringOK)
                {
                    _log.LogInformation("Stoppnavnet kunne ikke endres");
                    return BadRequest("Stoppnavnet kunne ikke endres");
                }
                return Ok("Stoppnavnet er endret");
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalideringen på server");
        }
    }
}
