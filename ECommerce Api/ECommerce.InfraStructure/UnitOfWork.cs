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

        public IAuthRepository Users { get; private set; }

        public IBaseRepository<Order> Orders { get; private set; }

        public IBaseRepository<Cart> Carts { get; private set; }

        public IBaseRepository<CartItems> CartItems { get; private set; }

        public IBaseRepository<Shipping> Shipping { get; private set; }

        public IBaseRepository<Payment> Payment { get; private set; }

        public IBaseRepository<Review> Reviews { get; private set; }
        public IBaseRepository<OrderItem> OrderItems { get; private set; }

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
            Users = new AuthRepository(_userManager, _jwtOptions, _roleManager, _httpContextAccessor);
            Categories = new BaseRepository<Category>(_context);
            Products = new ProductRepository(_context, _imageService);
            Orders = new BaseRepository<Order>(_context);
            Carts = new BaseRepository<Cart>(_context);
            CartItems = new BaseRepository<CartItems>(_context);
            Shipping = new BaseRepository<Shipping>(_context);
            Payment = new BaseRepository<Payment>(_context);
            Reviews = new BaseRepository<Review>(_context);
            OrderItems = new BaseRepository<OrderItem>(_context);
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
