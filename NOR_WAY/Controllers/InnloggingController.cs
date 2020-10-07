using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL.Interfaces;

namespace NOR_WAY.Controllers
{
    [Route("[controller]/[action]")]
    public class InnloggingController : ControllerBase
    {
        private readonly IInnloggingRepository _db;
        private ILogger<InnloggingController> _log;

        public InnloggingController(IInnloggingRepository db, ILogger<InnloggingController> log)
        {
            _db = db;
            _log = log;
        }

        public Task<ActionResult> LoggInn(string brukernavn, string passord)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> LoggUt()
        {
            throw new NotImplementedException();
        }

        //TODO: Midlertidig til vi vet om denne skal ligge her
        public Task<ActionResult> NyAdmin(string brukernavn, string passord)
        {
            throw new NotImplementedException();
        }
    }
}
