﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NOR_WAY.Model
{
    [ExcludeFromCodeCoverage]
    public class InnStopp
    {
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,50}")]
        public string Navn { get; set; }
    }
}