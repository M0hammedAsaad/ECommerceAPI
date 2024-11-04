using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Dtos
{
    public class RegisterDto
    {
        [MinLength(6, ErrorMessage = "MinLength must be more than 6"), MaxLength(100)]
        public string UserName { get; set; }
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(100)]
        //[DataType(DataType.Password)]
        public string Password { get; set; }
        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }

    }
}
