using Microsoft.AspNetCore.Http;

namespace ECommerce.Core.Helper
{
    public class ImageService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ImageService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> AddImage(IFormFile image)
        {
            var coverName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            var path = Path.Combine("wwwroot/ProductsImages", coverName);

            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            using var stream = File.Create(path);
            await image.CopyToAsync(stream);

            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                throw new InvalidOperationException("Unable to access the current HTTP request.");
            }

            var baseUrl = $"{request.Scheme}://{request.Host.Value}";
            return $"{baseUrl}/ProductsImages/{coverName}";
        }

        public Task DeleteImage(string imageUrl)
        {
            // Extract the file name from the URL
            var fileName = Path.GetFileName(imageUrl);

            // Construct the file path
            var filePath = Path.Combine("wwwroot/ProductsImages", fileName);

            // Check if the file exists and delete it
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            return Task.CompletedTask;
        }
    }

}
