using System.Text.Json.Serialization;

namespace ECommerce.Core.Dtos
{
    public class AuthResponseDto
    {

        public string? Message { get; set; }
        public bool IsAuthenticated { get; set; } = false;
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public List<string>? Role { get; set; }
        public string? TokenType { get; set; }
        [JsonIgnore]
        public string? RefreshToken { get; set; }

        public DateTime RefreshTokenExpiration { get; set; }
    }
}
