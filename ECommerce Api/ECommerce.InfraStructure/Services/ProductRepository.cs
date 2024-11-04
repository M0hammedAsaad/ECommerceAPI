using ECommerce.Core.Dtos;
using ECommerce.Core.Helper;
using ECommerce.Core.IServices;
using ECommerce.Core.Models;
using ECommerce.InfraStructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.InfraStructure.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ImageService _imageService;

        public ProductRepository(ApplicationDbContext context, ImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        public async Task AddProductAsync(ProductDto dto)
        {
            var coverName = await _imageService.AddImage(dto.Image);

            var Product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                ImageURL = coverName,
                CreatedAt = DateTime.Now,
                CategoryID = dto.CategoryID
            };

            await _context.Products.AddRangeAsync(Product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(int id)
        {
            var record = await _context.Products.FindAsync(id);
            _context.Products.Remove(record);
            await _context.SaveChangesAsync();

            await _imageService.DeleteImage(record.ImageURL);
        }

        public async Task<IEnumerable<GetProductDto>> GetAllProductsAsync()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Select(p => new GetProductDto
                {
                    ProductID = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    ImageURL = p.ImageURL,
                    CreatedAt = p.CreatedAt,
                    CategoryID = p.CategoryID,
                    CategoryName = p.Category.Name,
                    CategoryDescription = p.Category.Description

                }).ToListAsync();

            return products;

        }

        public async Task<GetProductDto> GetProductByIdAsync(int id)
        {
            var record = await _context.Products
             .Include(p => p.Category)
             .Select(p => new GetProductDto
             {
                 ProductID = p.Id,
                 Name = p.Name,
                 Description = p.Description,
                 Price = p.Price,
                 StockQuantity = p.StockQuantity,
                 ImageURL = p.ImageURL,
                 CreatedAt = p.CreatedAt,
                 CategoryID = p.CategoryID,
                 CategoryName = p.Category.Name,
                 CategoryDescription = p.Category.Description

             }).FirstOrDefaultAsync(c => c.ProductID == id);

            return record;
        }

        public async Task<Product> UpdateProductAsync(ProductDto dto, int id)
        {
            var oldProduct = await _context.Products.FindAsync(id);

            var coverName = await _imageService.AddImage(dto.Image);
            await _imageService.DeleteImage(oldProduct.ImageURL);

            oldProduct.Name = dto.Name;
            oldProduct.Description = dto.Description;
            oldProduct.Price = dto.Price;
            oldProduct.StockQuantity = dto.StockQuantity;
            oldProduct.ImageURL = coverName;
            oldProduct.CreatedAt = DateTime.Now;
            oldProduct.CategoryID = dto.CategoryID;


            _context.Products.Update(oldProduct);
            await _context.SaveChangesAsync();


            return oldProduct;
        }

    }
}
