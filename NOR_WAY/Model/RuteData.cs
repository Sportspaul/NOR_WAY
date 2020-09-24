using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NOR_WAY.Model
{
    public class RuteData
    {
        public List<string> Stoppene { get; set; }

        [RegularExpression(@"(NW)[0-9]{1,4}")]
        public string Linjekode { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Rutenavn { get; set; }

        [RegularExpression(@"[0-9]{1,4}")]
        public int minutterTilNesteStopp { get; set; }

        [RegularExpression(@"[0-9]{1,4}")]
        public int Startpris { get; set; }

        [RegularExpression(@"[0-9]{1,4}")]
        public int TilleggPerStopp { get; set; }
    }
}