using System;
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
    public class RuteStoppController : ControllerBase
    {
        private readonly IRuteStoppRepository _db;
        private ILogger<RuteStoppController> _log;
        private const string _innlogget = "Innlogget";
        private string melding;
        private string ugyldigValidering = "Feil i inputvalideringen på server";

        public RuteStoppController(IRuteStoppRepository db, ILogger<RuteStoppController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> FjernRuteStopp(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.FjernRuteStopp(id);
                if (!returOK)
                {
                    melding = $"RuteStopp med id: {id}, kunne ikke slettes";
                    _log.LogWarning(melding);
                    return BadRequest(melding);
                }
                melding = $"RuteStopp med id: {id}, ble slettet";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> HentRuteStopp(string linjekode)
        {
            if (ModelState.IsValid) {
                List<RuteStoppModel> ruteStoppListe = await _db.HentRuteStopp(linjekode);
                if (ruteStoppListe.IsNullOrEmpty()) { 
                    melding = $"Ingen RuteStopp ble funnet med linjekode: {linjekode}";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                return Ok(ruteStoppListe);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> HentEtRuteStopp(int id)
        {
            if (ModelState.IsValid)
            {
                NyRuteStopp ruteStopp = await _db.HentEtRuteStopp(id);
                if (ruteStopp == null)
                {
                    melding = $"Rutestoppet ble ikke funnet";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                return Ok(ruteStopp);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> NyRuteStopp(NyRuteStopp innRuteStopp)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.NyRuteStopp(innRuteStopp);
                if (!returOK)
                {
                    melding = $"Nytt RuteStopp kunne ikke lagres med verdiene: {innRuteStopp}";
                    _log.LogWarning(melding);
                    return BadRequest(melding);
                }
                melding = $"Nytt RuteStopp ble lagret med verdiene: {innRuteStopp}";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> OppdaterRuteStopp(NyRuteStopp ruteStoppOppdater)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOK = await _db.OppdaterRuteStopp(ruteStoppOppdater);
                if (!returOK)
                {
                    melding = $"Endringen av RuteStopp kunne ikke utføres med verdiene: {ruteStoppOppdater}";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                melding = $"Endringen av RuteStopp ble utført med verdiene: {ruteStoppOppdater}";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }
    }
}