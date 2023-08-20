using Pokemon.Models;

namespace Pokemon.DTO
{
    public class DTOOrderResponse
    {

      
        public string? Orderdate { get; set; }
        public string? Deliverydate { get; set; }
        public string? Totalprice { get; set; }
        public string? Note { get; set; }
        public string? Paymentmethod { get; set; }
        public string? OrderState { get; set; }

        //   public virtual Cart? Cart { get; set; }
        //public virtual OrderState? OrderState { get; set; }
        // public virtual PaymentMethod? Paymentmethod { get; set; }
        public List<DTOProductDetails> MyCart { get; set; }


    }
}
