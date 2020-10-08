using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NOR_WAY.Model
{
    // Modell for å ta imot verdier fra klienten for å fullføre kjøp av billett
    [ExcludeFromCodeCoverage]
    public class OrdreModel
    {
        // TODO: Lagt til OrdreId, modifiser koden så det funker
        [RegularExpression(@"[1-9][0-9]{0,8}")]
        public int OrdreId { get; set; } = 1;

        [RegularExpression(@"^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-z]{2,4}$")]
        public string Epost { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string StartStopp { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string SluttStopp { get; set; }

        [RegularExpression(@"(NW)[0-9]{1,4}")]
        public string Linjekode { get; set; }

        [RegularExpression(@"[0-9]{1,}")]
        public int AvgangId { get; set; }

        public List<string> Billettyper { get; set; }
    }
}