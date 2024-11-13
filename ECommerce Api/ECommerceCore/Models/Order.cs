using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }

        public Shipping Shipping { get; set; }
        public Payment Payment { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
