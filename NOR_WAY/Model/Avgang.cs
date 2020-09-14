using System;
namespace NOR_WAY.Model
{
    public class Avgang
    {
        public int AvgangId { get; set; }
        public string Rutenavn { get; set; }
        public string Linjekode { get; set; }
        public float Pris { get; set; }
        public int LedigePlasser { get; set; }
        public string Dato { get; set; }
        public string Avreise { get; set; }
        public string Ankomst { get; set; }
        public string Reisetid { get; set; }
    }
}
