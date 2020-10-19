using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NOR_WAY.Model
{
    [ExcludeFromCodeCoverage]
    public class NyAvgang
    {
        [RegularExpression(@"([0-9]{4})[-]([0-9]{2})[-]([0-9]{2})")]
        public string Dato { get; set; }

        [RegularExpression(@"([0-9]{2})[:]([0-9]{2})")]
        public string Tidspunkt { get; set; }

        [RegularExpression(@"[0-9]{1,2}")]
        public int SolgteBilletter { get; set; }

        [RegularExpression(@"(NW)[0-9]{1,4}")]
        public string Linjekode { get; set; }

        public override string ToString()
        {
            string utString = $"Dato: {Dato}, " +
                $"Tidspunkt: {Tidspunkt}, " +
                $"SolgteBilletter: {SolgteBilletter}, " +
                $"Linjekode {Linjekode}";
            return utString;
        }
    }
}