using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Internal;
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
            List<OrdreModel> ordreModelListe = await _db.HentOrdre(epost);
            if (ordreModelListe.IsNullOrEmpty())
            {
                melding = "Ingen ordre ble funnet";
                _log.LogWarning(melding);
                return NotFound(melding);
            }
            return Ok(ordreModelListe);
        }

        public Task<ActionResult> SlettOrdre(int id)
        {
            throw new NotImplementedException();
        }
    }
}