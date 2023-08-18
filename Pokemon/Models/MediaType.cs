using System;
using System.Collections.Generic;

namespace Pokemon.Models
{
    public partial class MediaType
    {
        public MediaType()
        {
            Media = new HashSet<Medium>();
        }

        public int MediatypeId { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Medium> Media { get; set; }
    }
}
