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
    public class AdminAvgangController
    {
        private readonly IAdminAvgangRepository _db;
        private ILogger<AdminAvgangController> _log;

        public AdminAvgangController(IAdminAvgangRepository db, ILogger<AdminAvgangController> log)
        {
            _db = db;
            _log = log;
        }

        public Task<ActionResult> FjernAvgang(int Id)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> HentAvganger(string linjekode, int side)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> NyAvgang(NyAvgang nyAvgang)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> OppdaterAvgang(Avgang avgang, int Id)
        {
            throw new NotImplementedException();
        }
    }
}
