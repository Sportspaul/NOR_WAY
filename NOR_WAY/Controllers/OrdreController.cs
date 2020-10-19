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
                    _log.LogInformation("Ordren kunne ikke lagres!");
                    return BadRequest("Ordren kunne ikke lagres!");
                }
                return Ok("Ordren ble lagret!");
            }
            _log.LogInformation("Feil i inputvalideringen");
            return BadRequest("Feil i inputvalidering på server");
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