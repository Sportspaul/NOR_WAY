using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.Controllers
{
    [Route("[controller]/[action]")]
    public class OrdreController : ControllerBase
    {
        private readonly IOrdreRepository _db;
        private ILogger<OrdreController> _log;
        private string melding;
        private string ugyldigValidering = "Feil i inputvalideringen på server";

        public OrdreController(IOrdreRepository db, ILogger<OrdreController> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<ActionResult> FullforOrdre(NyOrdre ordre)
        {
            if (ModelState.IsValid)
            {
                bool returOK = await _db.FullforOrdre(ordre);
                if (!returOK)
                {
                    melding = $"Ny ordre kunne ikke lagres med verdiene: {ordre}";
                    _log.LogError(melding);
                    return BadRequest(melding);
                }
                melding = $"Ny ordre ble lagret med verdiene: {ordre}";
                _log.LogInformation(melding);
                return Ok(melding);
            }
            _log.LogError(ugyldigValidering);
            return BadRequest(ugyldigValidering);
        }

        public async Task<ActionResult> HentOrdre(string epost)
        {
            var billetter = new List<string> { "Student", "Student", "Student", "Voksen", "Voksen", "Honnør", "Barn" };
            var ordre1 = new OrdreModel
            {
                Id = 1,
                Epost = "123@abc.no",
                StartStopp = "Bergen",
                SluttStopp = "Vadheim",
                Sum = "999",
                Linjekode = "NW431",
                Billettyper = billetter
            };

            List<OrdreModel> utOrdre = new List<OrdreModel> { ordre1 };
            return Ok(utOrdre);
        }

        public Task<ActionResult> SlettOrdre(int id)
        {
            throw new NotImplementedException();
        }
    }
}