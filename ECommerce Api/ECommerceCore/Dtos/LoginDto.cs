using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Dtos
{
    public class LoginDto
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
