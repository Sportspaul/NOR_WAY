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
    public class BillettyperController : ControllerBase
    {
        private readonly IBillettyperRepository _db;
        private ILogger<BillettyperController> _log;

        public BillettyperController(IBillettyperRepository db, ILogger<BillettyperController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> NyBillettype(Billettyper innBillettype)
        {
            // TODO: Legg til sjekk for Unauthorized
            if (ModelState.IsValid)
            {
                bool returOK = await _db.NyBillettype(innBillettype);
                if (!returOK)
                {
                    _log.LogInformation("Ny Billettype kunne ikke lagres!");
                    return BadRequest("Ny billettype kunne ikke lagres!");
                }
                return Ok("Ny billettype ble lagret!");
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalidering på server");
        }

        public async Task<ActionResult> HentAlleBillettyper()
        {
            List<Billettyper> billettypene = await _db.HentAlleBillettyper();
            return Ok(billettypene);
        }

        public async Task<ActionResult> OppdaterRabattsats(Billettyper oppdatertRabattsats)
        {
            // TODO: Legg til sjekk for Unauthorized
            if (ModelState.IsValid)
            {
                bool returOK = await _db.OppdaterRabattsats(oppdatertRabattsats);
                if (!returOK)
                {
                    _log.LogInformation("Endringen av Rabattsats kunne ikke utføres");
                    return NotFound("Endringen av rabattsats kunne ikke utføres");
                }
                return Ok("Rabattsats ble endret");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }
    }
}
