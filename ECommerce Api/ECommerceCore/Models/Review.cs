using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Core.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ApplicationUser")]
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public Product Product { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
