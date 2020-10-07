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
    public class AdminBillettyperController
    {
        private readonly IAdminBillettyperRepository _db;
        private ILogger<AdminBillettyperController> _log;

        public AdminBillettyperController(IAdminBillettyperRepository db, ILogger<AdminBillettyperController> log)
        {
            _db = db;
            _log = log;
        }

        public Task<ActionResult> NyBillettType()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> HentAlleBillettyper()
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> OppdaterBillettType(string navn)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> FjernBillettType(string navn)
        {
            throw new NotImplementedException();
        }
    }
}
