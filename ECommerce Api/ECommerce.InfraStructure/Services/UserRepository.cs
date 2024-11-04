using ECommerce.Core.Dtos;
using ECommerce.Core.Helper;
using ECommerce.Core.IServices;
using ECommerce.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.InfraStructure.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly JwtOptions _jwtOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepository(UserManager<ApplicationUser> userManager,
            JwtOptions jwtOptions, RoleManager<IdentityRole<int>> roleManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _jwtOptions = jwtOptions;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AuthResponseDto> LoginUserAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            var isPassword = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (user is null || !isPassword)
                return new AuthResponseDto { Message = "Email or Password is incorrect!" };

            var token = await GenerateJwtTokenAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var Respone = new AuthResponseDto
            {
                UserName = user.UserName,
                Email = user.Email,
                IsAuthenticated = true,
                Token = token,
                Role = roles.ToList(),
                TokenType = "Bearer"
            };

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activerefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                Respone.RefreshToken = activerefreshToken.Token;
                Respone.RefreshTokenExpiration = activerefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                Respone.RefreshToken = refreshToken.Token;
                Respone.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return Respone;
        }

        public async Task<AuthResponseDto> RegisterUserAsync(RegisterDto dto)
        {
            var email = await _userManager.FindByEmailAsync(dto.Email);
            if (email != null)
                return new AuthResponseDto { Message = "Email is already registered!" };

            var userName = await _userManager.FindByNameAsync(dto.UserName);
            if (userName != null)
                return new AuthResponseDto { Message = "Username is already registered!" };

            var user = new ApplicationUser()
            {
                UserName = dto.UserName,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                Address = dto.Address,
            };

            var Created = await _userManager.CreateAsync(user, dto.Password);
            if (!Created.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in Created.Errors)
                {
                    errors += $"{error.Description}\n";
                }
                return new AuthResponseDto { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "User");
            var token = await GenerateJwtTokenAsync(user);

            var Respone = new AuthResponseDto
            {
                UserName = user.UserName,
                Email = user.Email,
                IsAuthenticated = true,
                Token = token,
                Role = new List<string> { "User" },
                TokenType = "Bearer"
            };

            if (user.RefreshTokens.Any(t => t.IsActive))
            {
                var activerefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                Respone.RefreshToken = activerefreshToken.Token;
                Respone.RefreshTokenExpiration = activerefreshToken.ExpiresOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                Respone.RefreshToken = refreshToken.Token;
                Respone.RefreshTokenExpiration = refreshToken.ExpiresOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return Respone;
        }

        public async Task<string> AddRoleAsync(AddRoleDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.UserId.ToString());
            var role = await _roleManager.RoleExistsAsync(dto.Role);
            if (user == null || !role)
                return "Invalid User ID or Role";

            if (await _userManager.IsInRoleAsync(user, dto.Role))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, dto.Role);

            return result.Succeeded ? string.Empty : "Something went wrong";
        }

        private async Task<string> GenerateJwtTokenAsync(ApplicationUser user)
        {

            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim(ClaimTypes.Role, role));

            var claims = new List<Claim>()
            {
                    new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email,user.Email),
                    new Claim("uid",user.Id.ToString())
            }
            .Union(roleClaims)
            .Union(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtOptions.Issuer,
                audience: _jwtOptions.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtOptions.Expiration),
                signingCredentials: credentials

                );

            var Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return Token;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow,
            };
        }

        private void SetRefreshtokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = expires,
                HttpOnly = true
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
