using Microsoft.AspNetCore.Identity;

namespace ECommerce.Core.Models
{
    public class ApplicationUser : IdentityUser<int>
    {
        //public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<RefreshToken>? RefreshTokens { get; set; }
    }
}
