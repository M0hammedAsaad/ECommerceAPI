using AutoMapper;
using ECommerce.Core;
using ECommerce.Core.DTOs;
using ECommerce.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public OrderItemsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet("{orderId}/items")]
        public async Task<IActionResult> GetAllAsync(int orderId)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                return NotFound("Order Id is null or Not Found!");

            var items = await _unitOfWork.OrderItems.FindAsync(i => i.OrderId == orderId);
            if (items == null)
                return NotFound("No items for this Order!");


            var records = _mapper.Map<IEnumerable<OrderItemDTO>>(items);
            return Ok(records);
        }

        [HttpPost("{orderId}/items")]
        public async Task<IActionResult> AddOrderItems(int orderId, OrderItemCreateDTO dTO)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                return NotFound("Order Id is null or Not Found!");

            var product = await _unitOfWork.Products.GetProductByIdAsync(dTO.ProductId);
            if (product == null)
                return BadRequest("Product ID is null or Not Found!");

            var items = _mapper.Map<OrderItem>(dTO);
            items.OrderId = orderId;

            await _unitOfWork.OrderItems.AddAsync(items);

            var records = _mapper.Map<OrderItemDTO>(items);
            return Ok(records);
        }

        [HttpPut("{orderId}/items/{id}")]
        public async Task<IActionResult> UpdateItemAsync(int orderId, int id, OrderItemUpdateDTO dTO)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                return NotFound("Order Id is null or Not Found!");

            var item = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (item == null)
                return NotFound("No items for this Order!");

            var product = await _unitOfWork.Products.GetProductByIdAsync(dTO.ProductId);
            if (product == null)
                return BadRequest("Product ID is null or Not Found!");

            _mapper.Map(dTO, item);
            item.OrderId = orderId;

            await _unitOfWork.OrderItems.UpdateAsync(item);

            var records = _mapper.Map<OrderItemDTO>(item);
            return Ok(records);
        }

        [HttpDelete("{orderId}/items/{Id}")]
        public async Task<IActionResult> DeleteItemAsync(int orderId, int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(orderId);
            if (order == null)
                return NotFound("Order Id is null or Not Found!");

            var item = await _unitOfWork.OrderItems.GetByIdAsync(id);
            if (item == null)
                return NotFound("No items for this Order!");

            await _unitOfWork.OrderItems.DeleteAsync(item);


            var records = _mapper.Map<OrderItemDTO>(item);
            return Ok(records);
        }

    }
}
