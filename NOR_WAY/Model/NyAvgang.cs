﻿using System;
using System.ComponentModel.DataAnnotations;

namespace NOR_WAY.Model
{
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
    }
}