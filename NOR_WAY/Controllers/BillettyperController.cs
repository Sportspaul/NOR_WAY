﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Http;
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
        private const string _innlogget = "Innlogget";
        private string melding;
        private string ugyldigValidering = "Feil i inputvalideringen på server";

        public BillettyperController(IBillettyperRepository db, ILogger<BillettyperController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> NyBillettype(Billettyper innBillettype)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            int rabattsats = innBillettype.Rabattsats;
            if (ModelState.IsValid && rabattsats <= 100 && rabattsats >= 0)
            {
                bool returOK = await _db.NyBillettype(innBillettype);
                if (!returOK)
                {
                    melding = $"Ny Billettype kunne ikke lagres med verdiene: {innBillettype}";
                    _log.LogWarning(melding);
                    return BadRequest(melding);
                }
                melding = $"Ny billettype ble lagret med verdiene: {innBillettype}";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> HentAlleBillettyper()
        {
            List<Billettyper> billettypene = await _db.HentAlleBillettyper();
            if (billettypene.IsNullOrEmpty())
            {
                melding = "Ingen Billettyper ble funnet";
                _log.LogWarning(melding);
                return NotFound(melding);
            }
            return Ok(billettypene);
        }

        public async Task<ActionResult> HentEnBillettype(int id)
        {
            if (ModelState.IsValid)
            {
                Billettyper billettype = await _db.HentEnBillettype(id);
                if (billettype == null)
                {
                    melding = $"Billettypen med Id: {id}, ble ikke funnet";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                return Ok(billettype);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> OppdaterBillettype(Billettyper oppdatertBillettype)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            int rabattsats = oppdatertBillettype.Rabattsats;
            if (ModelState.IsValid && rabattsats <= 100 && rabattsats >= 0)
            {
                bool returOK = await _db.OppdaterBillettype(oppdatertBillettype);
                if (!returOK)
                {
                    melding = $"Endringen av Billettype kunne ikke utføres med verdiene: {oppdatertBillettype}";
                    _log.LogWarning(melding);
                    return NotFound(melding);
                }
                melding = $"Endringen av Billettype ble utført med verdiene: {oppdatertBillettype}";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogWarning(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }
    }
}