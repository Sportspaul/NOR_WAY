using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL;
using NOR_WAY.Model;

namespace NOR_WAY.Controllers
{
    [Route("[controller]/[action]")]
    public class BussController : ControllerBase
    {
        private readonly IBussRepository _db;
        private ILogger<BussController> _log;

        public BussController(IBussRepository db, ILogger<BussController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> HentAlleStopp()
        {
               List<Stopp> alleStopp = await _db.HentAlleStopp();
               return Ok(alleStopp); // returnerer alltid OK, null ved tom DB
        }

        public async Task<ActionResult> FinnMuligeStartStopp(InnStopp startStopp)
        {
            if(ModelState.IsValid)
            {
                List<Stopp> stopp = await _db.FinnMuligeStartStopp(startStopp);
                return Ok(stopp); // returnerer alltid OK, null ved tom DB
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalideringen på server");
        }

        public async Task<ActionResult> FinnMuligeSluttStopp(InnStopp sluttStopp)
        {
            if(ModelState.IsValid)
            {
                List<Stopp> alleStopp = await _db.FinnMuligeSluttStopp(sluttStopp);
                return Ok(alleStopp);
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalideringen på server");
        }

        public async Task<ActionResult> HentAlleBillettyper()
        {
            List<Billettyper> billettypene = await _db.HentAlleBillettyper();
            return Ok(billettypene);
        }

        public async Task<ActionResult> HentAlleRuter()
        {
          
                List<RuteData> rutene = await _db.HentAlleRuter();
                if(rutene == null)
                {
                    _log.LogInformation("Rutene ble ikke funnet");
                    return NotFound("Rutene ble ikke funnet");
                }
                return Ok(rutene);
           
        }


        public async Task<ActionResult> FinnNesteAvgang(AvgangParam input)
        {
            if(ModelState.IsValid)
            {
                Avgang nesteAvgang = await _db.FinnNesteAvgang(input);
                if (nesteAvgang == null) {
                    _log.LogInformation("Avgang ikke funnet");
                    return BadRequest("Avgang ikke funnet");
                }
                return Ok(nesteAvgang);
            }
                
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalideringen på server");
        }

        public async Task<ActionResult> FullforOrdre(KundeOrdre ordre)
        {
            if(ModelState.IsValid)
            {
                bool returOK = await _db.FullforOrdre(ordre);
                if(! returOK)
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
