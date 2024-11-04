using ECommerce.Core.Dtos;
using ECommerce.Core.Models;

namespace ECommerce.Core.IServices
{
    public interface IProductRepository
    {
        Task AddProductAsync(ProductDto dto);

        Task<IEnumerable<GetProductDto>> GetAllProductsAsync();

        Task<GetProductDto> GetProductByIdAsync(int id);

        Task DeleteProductAsync(int id);

        Task<Product> UpdateProductAsync(ProductDto dto, int id);
    }
}
