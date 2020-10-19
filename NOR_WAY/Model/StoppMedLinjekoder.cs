using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NOR_WAY.Model
{
    [ExcludeFromCodeCoverage]
    public class StoppMedLinjekoder
    {
        [RegularExpression(@"[1-9][0-9]{0,8}")]
        public int Id { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string Stoppnavn { get; set; }

        [RegularExpression(@"(NW)[0-9]{1,4}")]
        public string Linjekoder { get; set; }
    }
}