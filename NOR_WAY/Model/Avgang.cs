using System;
using System.ComponentModel.DataAnnotations;

namespace NOR_WAY.Model
{
    public class Avgang
    {
        public int AvgangId { get; set; }
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public string Rutenavn { get; set; }
        [RegularExpression(@"(NW)[0-9]{1,4}")]
        public string Linjekode { get; set; }
        public float Pris { get; set; }
        [RegularExpression(@"([0-9]{4})[-]([0-9]{2})[-]([0-9]{2})[ ]([0-9]{2})[:]([0-9]{2})[:](00)")]
        public string Avreise { get; set; }
        [RegularExpression(@"([0-9]{4})[-]([0-9]{2})[-]([0-9]{2})[ ]([0-9]{2})[:]([0-9]{2})[:](00)")]
        public string Ankomst { get; set; }
        public int Reisetid { get; set; }
    }
}
