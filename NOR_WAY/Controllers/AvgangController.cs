using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.Controllers
{
    [Route("[controller]/[action]")]
    public class AvgangController : ControllerBase
    {
        private readonly IAvgangRepository _db;
        private ILogger<AvgangController> _log;
        private const string _innlogget = "Innlogget";
        private string melding;
        private string ugyldigValidering = "Feil i inputvalideringen på server";

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
                    melding = $"Ingen avgang ble funnet";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                return Ok(nesteAvgang);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> FjernAvgang(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.FjernAvgang(id);
                if (!returOK)
                {
                    melding = $"Avgangen med id: {id}, kunne ikke slettes";
                    _log.LogWarning(melding);
                    return BadRequest(melding);
                }
                melding = $"Avgangen med id: {id}, ble slettet";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> HentAvganger(string linjekode, int sidenummer)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                List<AvgangModel> avganger = await _db.HentAvganger(linjekode, sidenummer);
                if (avganger.IsNullOrEmpty())
                {
                    melding = $"Listen med avganger ble ikke funnet for linjekode: {linjekode} og sidenummer: {sidenummer}";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                return Ok(avganger);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> HentEnAvgang(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                NyAvgang avgangen = await _db.HentEnAvgang(id);
                if (avgangen == null)
                {
                    melding = $"Avganen med id: {id}, kunne ikke hentes";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                _log.LogInformation($"Avgangen {avgangen} ble hentet");
                return Ok($"Avgangen {avgangen} ble hentet");
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> NyAvgang(NyAvgang nyAvgang)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.NyAvgang(nyAvgang);
                if (!returOK)
                {
                    melding = $"Ny avgang kunne ikke lagres med verdiene: {nyAvgang}";
                    _log.LogWarning(melding);
                    return BadRequest(melding);
                }
                melding = $"Ny avgang ble lagret med verdiene: {nyAvgang}";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> OppdaterAvgang(Avreisetid avreisetid)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.OppdaterAvgang(avreisetid);
                if (!returOK)
                {
                    melding = $"Endring av Avgang kunne ikke utføres med verdiene: {avreisetid}";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                melding = $"Endring av Avgang ble utført med verdiene: {avreisetid}";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }
    }
}