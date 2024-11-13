namespace ECommerce.Core.DTOs
{
    public class ShippingDTO
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string ShippingMethod { get; set; }
        public string TrackingNumber { get; set; }
        public string ShippingStatus { get; set; }
    }

    public class ShippingCreateDTO
    {
        public int OrderId { get; set; }
        public string ShippingMethod { get; set; }
        public string TrackingNumber { get; set; }
        public string ShippingStatus { get; set; }
    }

    public class ShippingUpdateDTO
    {
        public string ShippingStatus { get; set; }

    }
}
