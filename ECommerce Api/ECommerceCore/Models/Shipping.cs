namespace ECommerce.Core.Models
{
    public class Shipping
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string ShippingMethod { get; set; }
        public string TrackingNumber { get; set; }
        public string ShippingStatus { get; set; }

        public Order Order { get; set; }
    }
}
