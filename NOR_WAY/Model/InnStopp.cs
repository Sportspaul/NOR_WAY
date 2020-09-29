using System.ComponentModel.DataAnnotations;

namespace NOR_WAY.Model
{
    public class InnStopp
    {
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string Navn { get; set; }
    }
}