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
        public string Dato { get; set; }
        public string Tidspunkt { get; set; } 
        public bool AvreiseEtter { get; set; } // TODO: Se på mulighet for navnendring
    }
}
