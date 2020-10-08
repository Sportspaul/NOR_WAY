﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NOR_WAY.Model
{
    public class AvgangModel
    {
        [RegularExpression(@"([0-9]{4})[-]([0-9]{2})[-]([0-9]{2})")]
        public DateTime Avreise { get; set; }

        [RegularExpression(@"[0-9]{1,2}")]
        public int SolgteBilletter { get; set; } = 0;

        [RegularExpression(@"(NW)[0-9]{1,4}")]
        public string Linjekode { get; set; }
    }
}