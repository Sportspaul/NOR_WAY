using System;
using System.Collections.Generic;
using NOR_WAY.DAL;

namespace NOR_WAY.Model
{
    public class KundeOrdre
    {
        public string Epost { get; set; }
        public string StartStopp { get; set; }
        public string SluttStopp { get; set; }
        public int Sum { get; set; }

        // Foreign Keys
        public string Linjekode { get; set; } // Ruter FK
        public string Avganger { get; set; } // Avgang FK
        public List<string> Billettype { get; set; } // Til Ordrelinjer
    }
}
