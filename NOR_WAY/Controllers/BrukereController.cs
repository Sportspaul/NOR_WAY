using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.Controllers
{
    [Route("[controller]/[action]")]
    public class BrukereController : ControllerBase
    {
        private readonly IBrukereRepository _db;
        private ILogger<BrukereController> _log;

        public BrukereController(IBrukereRepository db, ILogger<BrukereController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> LoggInn(BrukerModel bruker)
        {
            if (ModelState.IsValid)
            {
                bool returOk = await _db.LoggInn(bruker);
                if (!returOk)
                {
                    _log.LogInformation("Innlogging feilet for bruker: " + bruker.Brukernavn);
                    return BadRequest("Innlogging feilet for bruker: " + bruker.Brukernavn);
                }
                return Ok(true);
            }
            _log.LogInformation("Feil i inputvalidering");
            return BadRequest("Feil i inputvalidering på server");
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
