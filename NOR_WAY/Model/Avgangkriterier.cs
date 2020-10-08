using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NOR_WAY.Model
{
    // Modell for å ta imot nødvendige verdier fra klienten for å beregne neste mulige avgang
    [ExcludeFromCodeCoverage]
    public class Avgangkriterier
    {
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string StartStopp { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string SluttStopp { get; set; }

        [RegularExpression(@"([0-9]{4})[-]([0-9]{2})[-]([0-9]{2})")]
        public string Dato { get; set; }

        [RegularExpression(@"([0-9]{2})[:]([0-9]{2})")]
        public string Tidspunkt { get; set; }

        public bool AvreiseEtter { get; set; }

        public List<string> Billettyper { get; set; }
    }
}