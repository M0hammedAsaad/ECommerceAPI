using Microsoft.AspNetCore.Http;

namespace ECommerce.Core.Dtos
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; } = 0;

        public int StockQuantity { get; set; }
        public int CategoryID { get; set; }
        public IFormFile Image { get; set; }

    }
}
