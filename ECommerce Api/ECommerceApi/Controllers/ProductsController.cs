using ECommerce.Core;
using ECommerce.Core.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products.GetAllProductsAsync();
            return Ok(products);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetProductByIdAsync(int id)
        {
            var record = await _unitOfWork.Products.GetProductByIdAsync(id);
            if (record == null)
                return BadRequest($"No Product with ID {id}");

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductAsync([FromForm] ProductDto dto)
        {

            var record = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryID);
            if (record == null)
                return NotFound($"No Category with ID {dto.CategoryID}");

            await _unitOfWork.Products.AddProductAsync(dto);
            return Ok();
        }

        [HttpDelete("id")]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            var record = await _unitOfWork.Products.GetProductByIdAsync(id);
            if (record == null)
                return BadRequest($"No Product with ID {id}");

            await _unitOfWork.Products.DeleteProductAsync(id);
            return Ok();
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateProductAsync(int id, [FromForm] ProductDto dto)
        {
            var record = await _unitOfWork.Products.GetProductByIdAsync(id);
            if (record == null)
                return BadRequest($"No Product with ID {id}");

            var ckeckCategory = await _unitOfWork.Categories.GetByIdAsync(dto.CategoryID);
            if (ckeckCategory == null)
                return NotFound($"No Category with ID {dto.CategoryID}");



            var newProduct = await _unitOfWork.Products.UpdateProductAsync(dto, id);
            return Ok("Updated Done");
        }
    }
}
