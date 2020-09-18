using System;
using System.ComponentModel.DataAnnotations;

namespace NOR_WAY.Model
{
    // TODO: Se på mulighet for navnendring
    public class AvgangParam
    {
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string StartStopp { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string SluttStopp { get; set; }
        [RegularExpression(@"([0-9]{4})[-]([0-9]{2})[-]([0-9]{2})")]
        public string Dato { get; set; }
        [RegularExpression(@"([0-9]{2})[:]([0-9]{2})[:](00)")]
        public string Tidspunkt { get; set; }
        public bool AvreiseEtter { get; set; } // TODO: Se på mulighet for navnendring
    }
}
