using ECommerce.Core.IServices;
using ECommerce.Core.Models;

namespace ECommerce.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Category> Categories { get; }
        IBaseRepository<Order> Orders { get; }
        IBaseRepository<Cart> Carts { get; }
        IBaseRepository<CartItems> CartItems { get; }
        IBaseRepository<Shipping> Shipping { get; }
        IBaseRepository<Payment> Payment { get; }
        IBaseRepository<Review> Reviews { get; }
        IBaseRepository<OrderItem> OrderItems { get; }

        IProductRepository Products { get; }
        IAuthRepository Users { get; }

        int Complete();
    }
}
