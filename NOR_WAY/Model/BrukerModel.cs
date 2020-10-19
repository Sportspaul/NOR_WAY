using System.Diagnostics.CodeAnalysis;

namespace NOR_WAY.Model
{
    //TODO: Legge til regex
    [ExcludeFromCodeCoverage]
    public class BrukerModel
    {
        public string Brukernavn { get; set; }

        public string Passord { get; set; }
    }
}