using System;
using System.Collections.Generic;

namespace Pokemon.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public DateTime? Orderdate { get; set; }
        public DateTime? Deliverydate { get; set; }
        public double? Totalprice { get; set; }
        public string? Note { get; set; }
        public int? CartId { get; set; }
        public int? PaymentmethodId { get; set; }
        public int? OrderStateId { get; set; }

        public virtual Cart? Cart { get; set; }
        public virtual OrderState? OrderState { get; set; }
        public virtual PaymentMethod? Paymentmethod { get; set; }
    }
}
