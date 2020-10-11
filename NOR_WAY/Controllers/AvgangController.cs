using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.Controllers
{
    [Route("[controller]/[action]")]
    public class AvgangController : ControllerBase
    {
        private readonly IAvgangRepository _db;
        private ILogger<AvgangController> _log;

        public AvgangController(IAvgangRepository db, ILogger<AvgangController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> FinnNesteAvgang(Avgangkriterier kriterier)
        {
            if (ModelState.IsValid)
            {
                Reisedetaljer nesteAvgang = await _db.FinnNesteAvgang(kriterier);
                if (nesteAvgang == null)
                {
                    _log.LogInformation("Avgang ikke funnet");
                    return NotFound("Avgang ikke funnet");
                }
                return Ok(nesteAvgang);
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalideringen på server");
        }

        public async Task<ActionResult> FjernAvgang(int id)
        {
            // TODO: Legg til sjekk for Unauthorized
            if (ModelState.IsValid)
            {
                bool returOK = await _db.FjernAvgang(id);
                if (!returOK)
                {
                    _log.LogInformation("Avgangen kunne ikke slettes!");
                    return BadRequest("Avgangen kunne ikke slettes!");
                }
                return Ok("Avgangen ble slettet!");
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalidering på server");
        }

        public async Task<ActionResult> HentAvganger(string linjekode, int sidenummer)
        {
            // TODO: Legg til sjekk for Unauthorized
            if (ModelState.IsValid) { 
                Regex linjekodeRegex = new Regex(@"(NW)[0-9]{1,4}");
                if (linjekodeRegex.IsMatch(linjekode))
                {
                    List<Avganger> avganger = await _db.HentAvganger(linjekode, sidenummer);
                    if (avganger == null)
                    {
                        _log.LogInformation("Listen med avganger ble ikke funnet");
                        return NotFound("Listen med avganger ble ikke funnet");
                    }
                    return Ok(avganger);
                }
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalideringen på server");
        }

        public Task<ActionResult> NyAvgang(AvgangModel nyAvgang)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> OppdaterAvgang(int Id, Avganger oppdaterAvgang)
        {
            throw new NotImplementedException();
        }
    }
}
