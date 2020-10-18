using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL;
using NOR_WAY.DAL.Interfaces;

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
            if (billettypene == null)
            {
                _log.LogInformation("Ingen Billettyper ble funnet");
                return NotFound("Ingen billettyper ble funnet");
            }
            return Ok(billettypene);
        }

        public async Task<ActionResult> HentEnBillettype(int id)
        {
            // TODO: Legg til sjekk for Unauthorized
            if (ModelState.IsValid)
            {
                Billettyper billettype = await _db.HentEnBillettype(id);
                if (billettype == null)
                {
                    _log.LogInformation("Billettypen ble ikke funnet");
                    return NotFound("Billettyper ble ikke funnet");
                }
                return Ok(billettype);
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalidering på server");
        }

        public async Task<ActionResult> OppdaterBillettype(Billettyper oppdatertBillettype)
        {
            // TODO: Legg til sjekk for Unauthorized
            if (ModelState.IsValid && oppdatertBillettype.Rabattsats <= 100 && oppdatertBillettype.Rabattsats >= 0)
            {
                bool returOK = await _db.OppdaterBillettype(oppdatertBillettype);
                if (!returOK)
                {
                    _log.LogInformation("Endringen av Billettype kunne ikke utføres");
                    return NotFound("Endringen av billettype kunne ikke utføres");
                }
                return Ok("Billettypen ble endret");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }
    }
}