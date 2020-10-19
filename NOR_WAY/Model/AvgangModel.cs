using System.Diagnostics.CodeAnalysis;

namespace NOR_WAY.Model
{
    [ExcludeFromCodeCoverage]
    public class AvgangModel
    {
        //TODO: Regex
        public int Id { get; set; }

        public string Avreise { get; set; }

        public int SolgteBilletter { get; set; }
    }
}