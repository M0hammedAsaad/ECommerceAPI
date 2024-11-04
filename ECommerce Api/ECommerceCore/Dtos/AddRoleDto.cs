using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Dtos
{
    public class AddRoleDto
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string Role { get; set; }
    }
}
