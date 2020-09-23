﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NOR_WAY.DAL;

namespace NOR_WAY.Model
{
    public class RuteData
    {
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string StoppNavn { get; set; }
        [RegularExpression(@"(NW)[0-9]{1,4}")]
        public string Linjekode { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Rutenavn { get; set; }
        [RegularExpression(@"[0-9]{1,4}")]
        public int minutterTilNesteStopp { get; set; }
        [RegularExpression(@"[0-9]{1,4}")]
        public int Startpris { get; set; }
        [RegularExpression(@"[0-9]{1,4}")]
        public int Stoppnummer { get; set; }
        [RegularExpression(@"[0-9]{1,4}")]
        public int TilleggPerStopp { get; set; }
    }
}
