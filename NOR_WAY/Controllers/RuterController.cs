using System.Collections.Generic;
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
        private string melding;
        private string ugyldigValidering = "Feil i inputvalideringen på server";

        public RuterController(IRuterRepository db, ILogger<RuterController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> FjernRute(string linjekode)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.FjernRute(linjekode);
                if (!returOK)
                {
                    melding = $"Ruten med linjekode: {linjekode}, kunne ikke slettes";
                    _log.LogWarning(melding);
                    return BadRequest(melding);
                }
                melding = $"Ruten med linjekode: {linjekode}, ble slettet";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> HentAlleRuter()
        {
            List<Ruter> rutene = await _db.HentAlleRuter();
            if (rutene == null)
            {
                melding = "Rutene ble ikke funnet";
                _log.LogWarning(melding);
                return NotFound(melding);
            }
            return Ok(rutene);
        }

        public async Task<ActionResult> HentRuterMedStopp()
        {
            List<RuteMedStopp> rutene = await _db.HentRuterMedStopp();
            if (rutene == null)
            {
                melding = "Rutene ble ikke funnet";
                _log.LogWarning(melding);
                return NotFound(melding);
            }
            return Ok(rutene);
        }

        public async Task<ActionResult> HentEnRute(string linjekode)
        {
            if (ModelState.IsValid)
            {
                Ruter rute = await _db.HentEnRute(linjekode);
                if (rute == null)
                {
                    melding = "Ruten ble ikke funnet";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                return Ok(rute);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> NyRute(Ruter rute)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.NyRute(rute);
                if (!returOK)
                {
                    melding = $"Ny Rute kunne ikke lagres med verdiene: {rute}";
                    _log.LogWarning(melding);
                    return BadRequest(melding);
                }
                melding = $"Ny Rute ble lagres med verdiene: {rute}";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> OppdaterRute(Ruter endretRute)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.OppdaterRute(endretRute);
                if (!returOK)
                {
                    melding = $"Endringen av Ruten med linjekode: {endretRute.Linjekode}, " +
                        $"kunne ikke utføres med verdiene: {endretRute}";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                melding = $"Endring av Ruten med linjekode: {endretRute.Linjekode}, " +
                    $"ble utfør med verdiene: {endretRute}";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }
    }
}