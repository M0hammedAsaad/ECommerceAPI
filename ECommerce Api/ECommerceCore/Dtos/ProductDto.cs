using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Core.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        [Range(0.1, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "StockQuantity must be greater than 0.")]
        public int StockQuantity { get; set; }
        public int CategoryID { get; set; }
        public IFormFile Image { get; set; }

    }
}
