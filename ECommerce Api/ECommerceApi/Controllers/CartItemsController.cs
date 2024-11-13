using AutoMapper;
using ECommerce.Core;
using ECommerce.Core.DTOs;
using ECommerce.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CartItemsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetAllAsync(int cartId)
        {
            var items = await _unitOfWork.CartItems.FindAsync(c => c.CartId == cartId);
            if (items == null)
                return NotFound("No items founded!");

            return Ok(items);
        }

        [HttpPost("{cartId}")]
        public async Task<IActionResult> AddAsync(int cartId, CartItemCreateDTO dTO)
        {

            var item = _mapper.Map<CartItems>(dTO);
            item.CartId = cartId;

            await _unitOfWork.CartItems.AddAsync(item);

            var record = _mapper.Map<CartItemDTO>(item);
            return Ok(record);
        }

        [HttpPut("{cartId}/items/{id}")]
        public async Task<IActionResult> UpdateAsync(int cartId, int id, CartItemUpdateDTO dTO)
        {
            var item = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (item == null)
                return NotFound("No items founded!");

            _mapper.Map(dTO, item);
            item.CartId = cartId;

            await _unitOfWork.CartItems.UpdateAsync(item);

            var record = _mapper.Map<CartItemDTO>(item);
            return Ok(record);
        }

        [HttpDelete("{cartId}/items/{id}")]
        public async Task<IActionResult> DeleteAsync(int cartId, int id)
        {
            var item = await _unitOfWork.CartItems.GetByIdAsync(id);
            if (item == null)
                return NotFound("No items founded!");

            await _unitOfWork.CartItems.DeleteAsync(item);

            var record = _mapper.Map<CartItemDTO>(item);
            return Ok(record);
        }
    }
}
