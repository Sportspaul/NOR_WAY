using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NOR_WAY.DAL;

namespace NOR_WAY.Model
{
    public class RuteModel
    {
        [RegularExpression(@"(NW)[0-9]{1,4}")]
        public string Linjekode { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string Rutenavn { get; set; }

        [RegularExpression(@"[0-9]{1,4}")]
        public int Startpris { get; set; }

        [RegularExpression(@"[0-9]{1,4}")]
        public int TilleggPerStopp { get; set; }

        [RegularExpression(@"[0-9]{1,3}")]
        public int Kapasitet { get; set; }

        public List<RuteStoppModel> RuteStopp { get; set; }
    }
}
