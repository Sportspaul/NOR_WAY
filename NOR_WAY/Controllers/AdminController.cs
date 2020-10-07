using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL;
using NOR_WAY.DAL.Interfaces;

namespace NOR_WAY.Controllers
{
    [Route("[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _db;
        private ILogger<AdminController> _log;

        public AdminController(IAdminRepository db, ILogger<AdminController> log)
        {
            _db = db;
            _log = log;
        }
    }
}
