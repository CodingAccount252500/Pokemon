using System;
using System.Collections.Generic;

namespace Pokemon.Models
{
    public partial class CartItem
    {
        public int CartItem1 { get; set; }
        public int? CartId { get; set; }
        public int? ProductId { get; set; }
        public double? TotalPrice { get; set; }
        public int? Quantity { get; set; }

        public virtual Cart? Cart { get; set; }
        public virtual Product? Product { get; set; }
    }
}
