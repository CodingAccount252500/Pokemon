using System;
using System.Collections.Generic;

namespace Pokemon.Models
{
    public partial class Product
    {
        public Product()
        {
            CartItems = new HashSet<CartItem>();
        }

        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? Price { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public int? CategoryProductId { get; set; }

        public virtual CategoryProduct? CategoryProduct { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
    }
}
