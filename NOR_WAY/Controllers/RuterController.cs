using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
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
    public class RuterController : ControllerBase
    {
        private readonly IRuterRepository _db;
        private ILogger<RuterController> _log;
        private const string _innlogget = "Innlogget";

        public RuterController(IRuterRepository db, ILogger<RuterController> log)
        {
            _db = db;
            _log = log;
        }
        
        public async Task<ActionResult> FjernRute(LinjekodeModel linjekodeModel)
        {
            // TODO: Legg til sjekk for Unauthorized
            if (ModelState.IsValid)
            {
                bool returOK = await _db.FjernRute(linjekodeModel.Linjekode);
                if (!returOK)
                {
                    _log.LogInformation("Ruten kunne ikke slettes!");
                    return BadRequest("Ruten kunne ikke slettes!");
                }
                return Ok("Ruten ble slettet!");
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalidering på server");
        }

        public async Task<ActionResult> HentAlleRuter()
        {
            List<Ruter> rutene = await _db.HentAlleRuter();
            if (rutene == null) {
                _log.LogInformation("Rutene ble ikke funnet");
                return NotFound("Rutene ble ikke funnet");
            }
            return Ok(rutene);
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

        public async Task<ActionResult> NyRute(RuteModel ruteModel)
        {
            // TODO: Legg til sjekk for Unauthorized
            if (ModelState.IsValid)
            {
                bool returOK = await _db.NyRute(ruteModel);
                if (!returOK)
                {
                    _log.LogInformation("Ny rute kunne ikke lagres!");
                    return BadRequest("Ny rute kunne ikke lagres!");
                }
                return Ok("Ny rute ble lagret!");
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalidering på server");
        }

        public Task<ActionResult> OppdaterRute(string linjekode, Ruter oppdatertRute)
        {
            throw new NotImplementedException();
        }
    }
}
