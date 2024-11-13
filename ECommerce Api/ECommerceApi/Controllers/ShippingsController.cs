using AutoMapper;
using ECommerce.Api.Filters;
using ECommerce.Core;
using ECommerce.Core.DTOs;
using ECommerce.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ShippingsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GerAllAsync()
        {
            var shipping = await _unitOfWork.Shipping.GetAllAsync();

            var records = _mapper.Map<IEnumerable<ShippingDTO>>(shipping);
            return Ok(records);
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateIdNotNullAttribute))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var shipping = await _unitOfWork.Shipping.GetByIdAsync(id);
            if (shipping == null)
                return NotFound("No shipping with this ID!");

            var record = _mapper.Map<ShippingDTO>(shipping);
            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(ShippingCreateDTO dTO)
        {

            var orderId = await _unitOfWork.Orders.GetByIdAsync(dTO.OrderId);
            if (orderId == null)
                return BadRequest("Order ID is null or not found!");

            var shipping = _mapper.Map<Shipping>(dTO);
            shipping.ShippingDate = DateTime.Now;
            shipping.DeliveryDate = DateTime.Now;

            await _unitOfWork.Shipping.AddAsync(shipping);

            var record = _mapper.Map<ShippingDTO>(shipping);
            return Ok(record);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidateIdNotNullAttribute))]
        public async Task<IActionResult> UpdateAsync(int id, ShippingUpdateDTO dTO)
        {
            var shipping = await _unitOfWork.Shipping.GetByIdAsync(id);
            if (shipping == null)
                return NotFound("No shipping with this ID!");

            _mapper.Map(dTO, shipping);
            shipping.DeliveryDate = DateTime.Now;

            await _unitOfWork.Shipping.UpdateAsync(shipping);

            var record = _mapper.Map<ShippingDTO>(shipping);
            return Ok(record);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateIdNotNullAttribute))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var shipping = await _unitOfWork.Shipping.GetByIdAsync(id);
            if (shipping == null)
                return NotFound("No shipping with this ID!");

            await _unitOfWork.Shipping.DeleteAsync(shipping);

            var record = _mapper.Map<ShippingDTO>(shipping);
            return Ok(record);
        }

    }
}
