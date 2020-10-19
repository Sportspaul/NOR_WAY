using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NOR_WAY.Model
{
    [ExcludeFromCodeCoverage]
    public class RuteStoppOppdatert
    {
        [RegularExpression(@"[0-9]{1,2}")]
        public int GammeltStoppNummer { get; set; }

        [RegularExpression(@"[0-9]{1,2}")]
        public int StoppNummer { get; set; }

        [RegularExpression(@"[0-9]{1,3}")]
        public int MinutterTilNesteStopp { get; set; }

        [RegularExpression(@"(NW)[0-9]{1,4}")]
        public string Linjekode { get; set; }
    }
}