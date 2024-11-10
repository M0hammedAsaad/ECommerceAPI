using ECommerce.Core.Dtos;
using ECommerce.Core.Helper;
using ECommerce.Core.IServices;
using ECommerce.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerce.InfraStructure.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly JwtOptions _jwtOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthRepository(UserManager<ApplicationUser> userManager,
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

        public async Task<AuthResponseDto> RefreshTokenAsync(string token)
        {
            var authRespone = new AuthResponseDto();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));
            if (user == null)
            {
                authRespone.Message = "Invalid token";
                return authRespone;
            }

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive)
            {
                authRespone.Message = "Inactive token";
                return authRespone;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtToken = await GenerateJwtTokenAsync(user);

            authRespone.IsAuthenticated = true;
            authRespone.Token = jwtToken;
            authRespone.TokenType = "Bearer";
            var roles = await _userManager.GetRolesAsync(user);
            authRespone.Role = roles.ToList();
            authRespone.UserName = user.UserName;
            authRespone.Email = user.Email;
            authRespone.RefreshToken = newRefreshToken.Token;
            authRespone.RefreshTokenExpiration = newRefreshToken.ExpiresOn;

            return authRespone;
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
                return false;

            var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActive) return false;

            refreshToken.RevokedOn = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return true;
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
    }
}
