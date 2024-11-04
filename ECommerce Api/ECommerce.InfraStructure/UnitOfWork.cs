using ECommerce.Core;
using ECommerce.Core.Helper;
using ECommerce.Core.IServices;
using ECommerce.Core.Models;
using ECommerce.InfraStructure.DataAccess;
using ECommerce.InfraStructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ECommerce.InfraStructure
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context;
        private readonly ImageService _imageService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly JwtOptions _jwtOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IBaseRepository<Category> Categories { get; private set; }

        public IProductRepository Products { get; private set; }

        public IUserRepository Users { get; private set; }

        public UnitOfWork(ApplicationDbContext context,
            ImageService imageService, UserManager<ApplicationUser> userManager,
            IOptions<JwtOptions> jwtOptions, RoleManager<IdentityRole<int>> roleManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _imageService = imageService;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtOptions = jwtOptions.Value;
            _httpContextAccessor = httpContextAccessor;
            Users = new UserRepository(_userManager, _jwtOptions, _roleManager, _httpContextAccessor);
            Categories = new BaseRepository<Category>(_context);
            Products = new ProductRepository(_context, _imageService);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
