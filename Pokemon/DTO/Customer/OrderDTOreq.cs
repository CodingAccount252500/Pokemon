namespace Pokemon.DTO.Customer
{
    public class OrderDTOreq
    {
        public int CartId { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Note { get; set; }
    }
}
