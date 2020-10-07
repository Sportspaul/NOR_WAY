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
    public class OrdreController
    {
        private readonly IAvgangRepository _db;
        private ILogger<OrdreController> _log;

        public OrdreController(IAvgangRepository db, ILogger<OrdreController> log)
        {
            _db = db;
            _log = log;
        }

        public Task<ActionResult> HentOrdre(string epost)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> SlettOrdre(int id)
        {
            throw new NotImplementedException();
        }
    }
}
