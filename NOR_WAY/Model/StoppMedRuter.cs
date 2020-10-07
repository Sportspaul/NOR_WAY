using System.Collections.Generic;

namespace NOR_WAY.Model
{
    public class StoppMedRuter
    {
        public int Id { get; set; }
        public string Stoppnavn { get; set; }
        public List<string> Linjekoder{get; set;}
    }
} 