using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
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
        private const string _innlogget = "Innlogget";
        private string melding;
        private string ugyldigValidering = "Feil i inputvalideringen på server";

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
                    HttpContext.Session.SetString(_innlogget, "");
                    melding = $"Innlogging feilet for bruker: {bruker.Brukernavn}";
                    _log.LogError(melding);
                    return BadRequest(melding);
                }
                HttpContext.Session.SetString(_innlogget, "Innlogget");
                return Ok(true);
            }
            _log.LogError(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public void LoggUt()
        {
            HttpContext.Session.SetString(_innlogget, "");
        }

        public bool AdminTilgang()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return false;
            }
            return true;
        }

        public async Task<ActionResult> NyAdmin(BrukerModel bruker)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_innlogget)))
            {
                return Unauthorized("Ikke innlogget");
            }
            if (ModelState.IsValid)
            {
                bool returOk = await _db.NyAdmin(bruker);
                if (!returOk)
                {
                    melding = $"Ny bruker kunne ikke lagres med brukernavn: {bruker.Brukernavn}";
                    _log.LogError(melding);
                    return BadRequest(melding);
                }
                melding = $"Ny bruker ble lagret med brukernavn: {bruker.Brukernavn}";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogError(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }
    }
}