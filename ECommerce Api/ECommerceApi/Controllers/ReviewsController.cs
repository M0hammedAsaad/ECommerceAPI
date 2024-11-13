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
    public class ReviewsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReviewsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var reviews = await _unitOfWork.Reviews.GetAllAsync();

            var records = _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
            return Ok(records);
        }

        [HttpGet("{id}")]
        [ServiceFilter(typeof(ValidateIdNotNullAttribute))]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null)
                return BadRequest("No review with this ID");

            var record = _mapper.Map<ReviewDTO>(review);
            return Ok(record);
        }

        [HttpGet("product/{productId}")]
        public async Task<IActionResult> GetByProductIdReviewAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(productId);
            if (product == null)
                return BadRequest("No product with this ID!");

            var reviews = await _unitOfWork.Reviews.FindAsync(c => c.ProductId == productId);
            if (reviews == null)
                return NotFound("No review with this ID");

            var record = _mapper.Map<IEnumerable<ReviewDTO>>(reviews);
            return Ok(record);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(ReviewCreateDTO dTO)
        {
            var review = _mapper.Map<Review>(dTO);
            review.CreatedAt = DateTime.Now;

            await _unitOfWork.Reviews.AddAsync(review);

            var record = _mapper.Map<ReviewDTO>(review);
            return Ok(record);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidateIdNotNullAttribute))]
        public async Task<IActionResult> UpdateAsync(int id, ReviewUpdateDTO dTO)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null)
                return NotFound("No review with this ID");

            _mapper.Map(dTO, review);
            review.CreatedAt = DateTime.Now;

            await _unitOfWork.Reviews.UpdateAsync(review);

            var record = _mapper.Map<ReviewDTO>(review);
            return Ok(record);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateIdNotNullAttribute))]
        public async Task<IActionResult> UpdateAsync(int id)
        {
            var review = await _unitOfWork.Reviews.GetByIdAsync(id);
            if (review == null)
                return NotFound("No review with this ID");

            await _unitOfWork.Reviews.DeleteAsync(review);

            var record = _mapper.Map<ReviewDTO>(review);
            return Ok(record);
        }
    }
}
