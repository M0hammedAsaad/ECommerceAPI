using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<RefreshToken>? RefreshTokens { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Order> Orders { get; set; }
        public Cart Cart { get; set; }
    }
}
