﻿using System;
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

        public Task<ActionResult> NyBillettType(Billettyper nyBillettype)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> HentAlleBillettyper()
        {
            List<Billettyper> billettypene = await _db.HentAlleBillettyper();
            return Ok(billettypene);
        }

        public Task<ActionResult> OppdaterRabattsats(Billettyper oppdatertRabattsats)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> FjernBillettType(string navn)
        {
            throw new NotImplementedException();
        }
    }
}
