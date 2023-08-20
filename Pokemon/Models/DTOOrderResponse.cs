namespace Pokemon.Models
{
    public class DTOOrderResponse
    {

       
        public DateTime? Orderdate { get; set; }
        public DateTime? Deliverydate { get; set; }
        public double? Totalprice { get; set; }
        public string? Note { get; set; }
        public int? CartId { get; set; }
        public int? PaymentmethodId { get; set; }
        public int? OrderStateId { get; set; }

    }
}
