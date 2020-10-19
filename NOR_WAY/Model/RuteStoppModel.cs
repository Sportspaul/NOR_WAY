using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NOR_WAY.Model
{
    [ExcludeFromCodeCoverage]
    public class RuteStoppModel
    {
        public int Id { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string Stoppnavn { get; set; }

        [RegularExpression(@"[0-9]{1,2}")]
        public int StoppNummer { get; set; }

        [RegularExpression(@"[0-9]{1,3}")]
        public int MinutterTilNesteStopp { get; set; }
    }
}