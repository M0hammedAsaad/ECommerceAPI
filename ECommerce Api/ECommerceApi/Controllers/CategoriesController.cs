using AutoMapper;
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
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize]
        [SensitiveAction]
        public async Task<IActionResult> GetAllAsync()
        {
            var record = await _unitOfWork.Categories.GetAllAsync(x => x.Name);
            return Ok(record);
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var record = await _unitOfWork.Categories.GetByIdAsync(id);
            if (record == null)
                return NotFound($"ID {id} Not Founded");

            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(CategoryDTO dto)
        {
            var catergory = _mapper.Map<Category>(dto);
            catergory.CreatedAt = DateTime.UtcNow;

            await _unitOfWork.Categories.AddAsync(catergory);
            return Ok(catergory);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] CategoryDTO dto)
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

        [HttpDelete("{id}")]
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
