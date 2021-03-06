﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.Controllers
{
    [Route("[controller]/[action]")]
    public class StoppController : ControllerBase
    {
        private readonly IStoppRepository _db;
        private ILogger<StoppController> _log;
        private const string _innlogget = "Innlogget";
        private string melding;
        private string ugyldigValidering = "Feil i inputvalideringen på server";

        public StoppController(IStoppRepository db, ILogger<StoppController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> FinnMuligeStartStopp(StoppModel sluttStopp)
        {
            if (ModelState.IsValid)
            {
                List<Stopp> stoppListe = await _db.FinnMuligeStartStopp(sluttStopp);
                if (stoppListe.IsNullOrEmpty())
                {
                    melding = $"Ingen mulige StartStopp ble funnet for SluttStopp: {sluttStopp.Navn}";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                return Ok(stoppListe); // returnerer alltid OK, null ved tom DB
            }    
             _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> FinnMuligeSluttStopp(StoppModel startStopp)
        {
            if (ModelState.IsValid)
            {
                List<Stopp> stoppListe = await _db.FinnMuligeSluttStopp(startStopp);
                if (stoppListe.IsNullOrEmpty())
                {
                    melding = $"Ingen mulige SluttStopp ble funnet for StartStopp: {startStopp.Navn}";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                return Ok(stoppListe);
            }
            
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> HentEtStopp(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                Stopp stopp = await _db.HentEtStopp(id);
                if (stopp == null)
                {
                    _log.LogInformation("Stoppet ble ikke funnet");
                    return NotFound("Stoppet ble ikke funnet");
                }
                return Ok(stopp);
            }
            _log.LogInformation("Feil i inputvalideringen på server");
            return BadRequest("Feil i inputvalidering på server");
        }


        public async Task<ActionResult> HentAlleStopp()
        {
            List<Stopp> alleStopp = await _db.HentAlleStopp();
            if (alleStopp.IsNullOrEmpty())
            {
                melding = "Ingen stopp ble funnet";
                _log.LogWarning(melding);
                return NotFound(melding);
            }
            return Ok(alleStopp); // returnerer alltid OK, null ved tom DB
        }

        // TODO: Sjekke at dette stoppnavnet finnes alt
        public async Task<ActionResult> OppdaterStoppnavn(Stopp innStopp)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool EndringOK = await _db.OppdaterStoppnavn(innStopp);
                if (!EndringOK)
                {
                    melding = $"Endring av Stoppnavn kunne ikke utføres med verdiene: {innStopp}";
                    _log.LogWarning(melding);
                    return BadRequest(melding);
                }
                melding = $"Endring av Stoppnavn ble utført med verdiene: {innStopp}";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> HentAlleStoppMedRuter()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                List<StoppMedLinjekoder> stoppMedLinjekoder = await _db.HentAlleStoppMedRuter();
                if (stoppMedLinjekoder.IsNullOrEmpty())
                {
                    melding = "Ingen Stopp ble funnet";
                    _log.LogWarning(melding);
                    return BadRequest(melding);
                }
                return Ok(stoppMedLinjekoder);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }
    }
}