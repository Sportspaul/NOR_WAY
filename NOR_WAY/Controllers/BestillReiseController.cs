using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.Controllers
{
    [Route("[controller]/[action]")]
    public class BestillReiseController : ControllerBase
    {
        private readonly IBestillReiseRepository _db;
        private ILogger<BestillReiseController> _log;

        public BestillReiseController(IBestillReiseRepository db, ILogger<BestillReiseController> log)
        {
            _db = db;
            _log = log;
        }

        
    }
}