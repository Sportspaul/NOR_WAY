using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NOR_WAY.Model
{
    [ExcludeFromCodeCoverage]
    public class Avreisetid
    {
        [RegularExpression(@"[0-9]{1,8}")]
        public int Id { get; set; }

        [RegularExpression(@"([0-9]{4})[-]([0-9]{2})[-]([0-9]{2})")]
        public string Dato { get; set; }

        [RegularExpression(@"([0-9]{2})[:]([0-9]{2})")]
        public string Tidspunkt { get; set; }

        public override string ToString()
        {
            string utString = $"Id: {Id}, Dato: {Dato}, Tidspunkt: {Tidspunkt}";
            return utString;
        }
    }
}