using System;
using System.ComponentModel.DataAnnotations;

namespace NOR_WAY.Model
{
    // Modell for å sende Avgangsinformasjon til klienten
    public class Avgang
    {
        [RegularExpression(@"[0-9]{1,}")]
        public int AvgangId { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Rutenavn { get; set; }

        [RegularExpression(@"(NW)[0-9]{1,4}")]
        public string Linjekode { get; set; }

        [RegularExpression(@"[0-9]{1,4}")]
        public float Pris { get; set; }

        [RegularExpression(@"([0-9]{4})[-]([0-9]{2})[-]([0-9]{2})[ ]([0-9]{2}[:][0-9]{2}))")]
        public string Avreise { get; set; }

        [RegularExpression(@"([0-9]{4})[-]([0-9]{2})[-]([0-9]{2})[ ]([0-9]{2}[:][0-9]{2}))")]
        public string Ankomst { get; set; }

        [RegularExpression(@"[0-9]{1,}")]
        public int Reisetid { get; set; }
    }
}
