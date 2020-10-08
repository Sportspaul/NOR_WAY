using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NOR_WAY.DAL.Interfaces;
using NOR_WAY.Model;

namespace NOR_WAY.DAL.Repositories
{
    [ExcludeFromCodeCoverage]
    public class BestillReiseRepository : IBestillReiseRepository
    {
        private readonly BussContext _db;
        private ILogger<BestillReiseRepository> _log;

        public BestillReiseRepository(BussContext db, ILogger<BestillReiseRepository> log)
        {
            _db = db;
            _log = log;
        }


        
    }
}