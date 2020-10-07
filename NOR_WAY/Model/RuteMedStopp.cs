using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NOR_WAY.Model
{
    [ExcludeFromCodeCoverage]
    public class RuteMedStopp
    {
        public List<string> Stoppene { get; set; }

        public List<int> MinutterTilNesteStopp { get; set; }

        [RegularExpression(@"(NW)[0-9]{1,4}")]
        public string Linjekode { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Rutenavn { get; set; }

        [RegularExpression(@"[0-9]{1,4}")]
        public int Startpris { get; set; }

        [RegularExpression(@"[0-9]{1,4}")]
        public int TilleggPerStopp { get; set; }
    }
}