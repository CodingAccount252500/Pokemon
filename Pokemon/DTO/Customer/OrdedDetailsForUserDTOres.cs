namespace Pokemon.DTO.Customer
{
    public class OrdedDetailsForUserDTOres
    {
        public string OrderDate { get; set; }

        public string DeliveryDate { get; set; }

        public string TotalPrice { get; set; }

        public string Note { get; set; }

        public string OrderStatus { get; set; }

        public string IsApproved { get; set; }

        public List<OrderCatrItemDTOreq> MyCart { get; set; }
    }
}
