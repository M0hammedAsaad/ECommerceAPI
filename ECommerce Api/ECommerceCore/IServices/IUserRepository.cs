using ECommerce.Core.Dtos;

namespace ECommerce.Core.IServices
{
    public interface IUserRepository
    {
        Task<AuthResponseDto> RegisterUserAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginUserAsync(LoginDto dto);
        Task<string> AddRoleAsync(AddRoleDto dto);
    }
}
