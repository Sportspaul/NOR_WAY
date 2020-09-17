﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL;
using NOR_WAY.Model;

namespace NOR_WAY.Controllers
{
    [Route("[controller]/[action]")]
    public class BussController : ControllerBase
    {
        private readonly IBussRepository _db;
        private ILogger<BussController> _log;

        public BussController(IBussRepository db, ILogger<BussController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<List<Stopp>> HentAlleStopp()
        {
            return await _db.HentAlleStopp();
        }

        public async Task<List<Billettyper>> HentAlleBillettyper()
        {
            return await _db.HentAlleBillettyper();
        }

        public async Task<Avgang> FinnNesteAvgang(AvgangParam param)
        {
            return await _db.FinnNesteAvgang(param);
        }

        public async Task<bool> FullforOrdre(KundeOrdre ordre)
        {
            return await _db.FullforOrdre(ordre);
        }
    }
}
