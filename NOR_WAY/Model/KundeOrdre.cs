using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NOR_WAY.DAL;

namespace NOR_WAY.Model
{
    public class KundeOrdre
    {
        [RegularExpression(@"^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-z]{2,4}$")]
        public string Epost { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string StartStopp { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string SluttStopp { get; set; }
        public int Sum { get; set; }

        // Foreign Keys
        [RegularExpression(@"(NW)[0-9]{1,4}")] //Linjekoden starter med NW for Norway
        public string Linjekode { get; set; } // Ruter FK
        public string Avganger { get; set; } // Avgang FK //TODO: legge til RegEx når det er klart hva gyldig format er
        public List<string> Billettype { get; set; } // Til Ordrelinjer
    }
}
