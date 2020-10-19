using System.ComponentModel.DataAnnotations;

namespace NOR_WAY.Model
{
    public class NyRuteStopp
    {
        public int Id { get; set; }

        [RegularExpression(@"[0-9]{1,2}")]
        public int StoppNummer { get; set; }

        [RegularExpression(@"[0-9]{1,3}")]
        public int MinutterTilNesteStopp { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string Stoppnavn { get; set; }

        [RegularExpression(@"(NW)[0-9]{1,4}")]
        public string Linjekode { get; set; }

        public override string ToString()
        {
            string utString = $"Id: {Id}, " +
                $"StoppNummer: {StoppNummer}, " +
                $"MinutterTilNesteStopp: {MinutterTilNesteStopp}, " +
                $"Stoppnavn: {Stoppnavn}, " +
                $"Linjekode: {Linjekode}";
            return utString;
        }
    }
}