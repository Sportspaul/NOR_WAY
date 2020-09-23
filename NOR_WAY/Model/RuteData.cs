using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.DAL;

namespace NOR_WAY.Model
{
    public class RuteData
    {
        public string StoppNavn { get; set; }
        public string Linjekode { get; set; }
        public string Rutenavn { get; set; }
        public int minutterTilNesteStopp { get; set; }
        public int pris { get; set; }
        public int TilleggPerStopp { get; set; }
        
    }
}
