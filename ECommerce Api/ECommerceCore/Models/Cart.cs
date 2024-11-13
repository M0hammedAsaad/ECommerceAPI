using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Core.Models
{
    public class Cart
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
        public ICollection<CartItems> CartItems { get; set; }
    }
}
