using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL.Interfaces;

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
