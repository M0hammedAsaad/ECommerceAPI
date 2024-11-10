using ECommerce.Core.Dtos;

namespace ECommerce.Core.IServices
{
    public interface IAuthRepository
    {
        Task<AuthResponseDto> RegisterUserAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginUserAsync(LoginDto dto);
        Task<string> AddRoleAsync(AddRoleDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);
    }
}
