using ECommerce.Core.Dtos;
using ECommerce.Core.Models;

namespace ECommerce.Core.IServices
{
    public interface IAuthRepository
    {
        Task<AuthResponseDto> RegisterUserAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginUserAsync(LoginDto dto);
        Task<string> AddRoleAsync(AddRoleDto dto);
        Task<AuthResponseDto> RefreshTokenAsync(string token);
        Task<bool> RevokeTokenAsync(string token);

        Task<ApplicationUser> GetByIDAsync(int id);
    }
}
