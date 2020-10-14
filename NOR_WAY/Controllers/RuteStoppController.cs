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
    public class RuteStoppController : ControllerBase
    {
        private readonly IRuteStoppRepository _db;
        private ILogger<RuteStoppController> _log;

        public RuteStoppController(IRuteStoppRepository db, ILogger<RuteStoppController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> FjernRuteStopp(int id)
        {
            // TODO: Legg til sjekk for Unauthorized
            if (ModelState.IsValid)
            {
                bool returOK = await _db.FjernRuteStopp(id);
                if (!returOK)
                {
                    _log.LogInformation("RuteStopp kunne ikke slettes!");
                    return BadRequest("Rutestoppet kunne ikke slettes!");
                }
                return Ok("Rutestoppet ble slettet!");
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalidering på server");
        }

        public Task<ActionResult> HentAlleRuteStopp()
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> HentRuteStopp(string linjekode)
        {
            List<RuteStoppModel> ruteStoppListe = await _db.HentRuteStopp(linjekode);
            if (ruteStoppListe.Count == 0)
            {
                _log.LogInformation("Ingen RuteStopp ble funnet");
                return NotFound("Ingen Rute stoppene ble funnet");
            }
            return Ok(ruteStoppListe);
        }

        public async Task<ActionResult> NyRuteStopp(NyRuteStopp innRuteStopp)
        {
            // TODO: Legg til sjekk for Unauthorized
            if (ModelState.IsValid)
            {
                bool returOK = await _db.NyRuteStopp(innRuteStopp);
                if (!returOK)
                {
                    _log.LogInformation("Nytt RuteStopp kunne ikke lagres!");
                    return BadRequest("Nytt rutestopp kunne ikke lagres!");
                }
                return Ok("Nytt rutestopp ble lagret!");
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalidering på server");
        }

        public async Task<ActionResult> OppdaterRuteStopp(NyRuteStopp ruteStoppOppdater)
        {
            // TODO: Legg til sjekk for Unauthorized
            if (ModelState.IsValid)
            {
                bool returOK = await _db.OppdaterRuteStopp(ruteStoppOppdater);
                if (!returOK)
                {
                    _log.LogInformation("Endringen av RuteStopp kunne ikke utføres");
                    return NotFound("Endringen av rutestoppet kunne ikke utføres");
                }
                return Ok("Rutestoppet ble endret");
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
        }
    }
}
