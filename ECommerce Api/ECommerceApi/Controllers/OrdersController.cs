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
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var records = await _unitOfWork.Orders.GetAllAsync();
            return Ok(records);
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateIdNotNullAttribute))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var record = await _unitOfWork.Orders.GetByIdAsync(id);
            if (record == null)
                return NotFound("No Oders with this ID");

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(OrderCreateDTO DTO)
        {
            var user = await _unitOfWork.Users.GetByIDAsync(DTO.UserId);
            if (user == null)
                return BadRequest("User Id is null or notfound");

            var order = _mapper.Map<Order>(DTO);
            order.OrderDate = DateTime.Now;

            await _unitOfWork.Orders.AddAsync(order);

            var record = _mapper.Map<OrderDTO>(order);
            return Ok(record);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidateIdNotNullAttribute))]
        public async Task<IActionResult> UpdateAsync(int id, OrderUpdateDTO DTO)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return NotFound("No Oders with this ID");

            _mapper.Map(DTO, order);
            order.OrderDate = DateTime.Now;

            await _unitOfWork.Orders.UpdateAsync(order);

            var record = _mapper.Map<OrderDTO>(order);
            return Ok(record);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateIdNotNullAttribute))]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);
            if (order == null)
                return NotFound("No Oders with this ID");

            await _unitOfWork.Orders.DeleteAsync(order);

            var record = _mapper.Map<OrderDTO>(order);
            return Ok(record);
        }

        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetAllOrdersForUserAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIDAsync(userId);

            if (user == null)
                return NotFound("User Id is null or notfound");

            var orders = await _unitOfWork.Orders.FindAsync(x => x.UserId == userId);

            var records = _mapper.Map<IEnumerable<OrderDTO>>(orders);
            return Ok(records);
        }
    }
}
