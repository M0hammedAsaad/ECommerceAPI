using ECommerce.Core;
using ECommerce.Core.Dtos;
using ECommerce.Core.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers.Security
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto dto)
        {
            var result = await _unitOfWork.Users.RegisterUserAsync(dto);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshtokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDto dto)
        {
            var record = await _unitOfWork.Users.LoginUserAsync(dto);
            if (!record.IsAuthenticated)
                return BadRequest(record.Message);
            if (!string.IsNullOrEmpty(record.RefreshToken))
                SetRefreshtokenInCookie(record.RefreshToken, record.RefreshTokenExpiration);

            return Ok(record);
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] AddRoleDto dto)
        {
            var result = await _unitOfWork.Users.AddRoleAsync(dto);
            if (result != string.Empty)
                return BadRequest(result);

            return Ok("user add to role successful");
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _unitOfWork.Users.RefreshTokenAsync(refreshToken);
            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            SetRefreshtokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            return Ok(result);
        }

        [HttpPost("revokeToken")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] RevokeTokenDTO dto)
        {
            var token = dto.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _unitOfWork.Users.RevokeTokenAsync(token);

            if (!result)
                return BadRequest("Invalid token!");

            return Ok();
        }


        private void SetRefreshtokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = expires.ToLocalTime(),
                HttpOnly = true
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
