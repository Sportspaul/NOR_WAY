using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.Controllers
{
    [Route("[controller]/[action]")]
    public class BestillReiseController : ControllerBase
    {
        private readonly IBestillReiseRepository _db;
        private ILogger<BestillReiseController> _log;

        public BestillReiseController(IBestillReiseRepository db, ILogger<BestillReiseController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> FinnNesteAvgang(AvgangParam input)
        {
            if (ModelState.IsValid)
            { 
                Avgang nesteAvgang = await _db.FinnNesteAvgang(input);
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

        public async Task<ActionResult> FullforOrdre(KundeOrdre ordre)
        {
            if (ModelState.IsValid)
            {
                bool returOK = await _db.FullforOrdre(ordre);
                if (!returOK)
                {
                    _log.LogInformation("Ordren kunne ikke lagres!");
                    return BadRequest("Ordren kunne ikke lagres!");
                }
                return Ok("Ordren ble lagret!");
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalidering på server");
        }
    }
}