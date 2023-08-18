using System;
using System.Collections.Generic;

namespace Pokemon.Models
{
    public partial class Medium
    {
        public int MediaId { get; set; }
        public string? MediaPath { get; set; }
        public bool? IsMainMedia { get; set; }
        public int? MediatypeId { get; set; }
        public int? ProductId { get; set; }

        public virtual MediaType? Mediatype { get; set; }
    }
}
