using AutoMapper;
using ECommerce.Api.Filters;
using ECommerce.Core;
using ECommerce.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CartsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserCartsAsync(int userId)
        {
            var cart = await _unitOfWork.Carts.FindAsync(c => c.UserId == userId);
            if (cart == null)
                return NotFound("No Cart for this User");

            return Ok(cart);
        }

        [HttpPost("{userId}")]
        public async Task<IActionResult> AddAsync(int userId)
        {
            var cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.Now,
            };

            await _unitOfWork.Carts.AddAsync(cart);
            return Ok(cart);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateIdNotNullAttribute))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(id);
            if (cart == null)
                return NotFound("No Cart for this User");

            await _unitOfWork.Carts.DeleteAsync(cart);

            return Ok(cart);
        }
    }
}
