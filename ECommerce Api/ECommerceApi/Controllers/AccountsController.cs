using ECommerce.Core;
using ECommerce.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
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

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginUserAsync([FromBody] LoginDto dto)
        {
            var record = await _unitOfWork.Users.LoginUserAsync(dto);
            if (!record.IsAuthenticated)
                return BadRequest(record.Message);

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
    }
}
