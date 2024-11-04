using ECommerce.Api.Filters;
using ECommerce.Core;
using ECommerce.Core.Dtos;
using ECommerce.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        //[Authorize]
        [SensitiveAction]
        public async Task<IActionResult> GetAllAsync()
        {
            var record = await _unitOfWork.Categories.GetAllAsync(x => x.Name);
            return Ok(record);
        }

        //[Authorize]
        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var record = await _unitOfWork.Categories.GetByIdAsync(id);
            if (record == null)
                return NotFound($"ID {id} Not Founded");

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromForm] CategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoey = new Category
            {
                Name = dto.Name,
                Description = dto.Description,
                CreatedAt = DateTime.Now,
            };
            await _unitOfWork.Categories.AddAsync(categoey);
            return Ok(categoey);
        }

        [HttpPut("id")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CategoryDto dto)
        {
            var record = await _unitOfWork.Categories.GetByIdAsync(id);
            if (record == null)
                return BadRequest($"ID {id} Not Founded");

            record.Name = dto.Name;
            record.Description = dto.Description;
            record.CreatedAt = DateTime.Now;

            await _unitOfWork.Categories.UpdateAsync(record);
            return Ok(record);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(int id)
        {

            var record = await _unitOfWork.Categories.GetByIdAsync(id);

            if (record == null)
                return NotFound($"ID {id} Not Founded");

            await _unitOfWork.Categories.DeleteAsync(id);
            return Ok(record);
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByNameAsync(string name)
        {
            var record = await _unitOfWork.Categories.FindByNameAsync(c => c.Name == name);
            if (record == null)
                return NotFound($"No Category with Name {name}");

            return Ok(record);
        }
    }
}
