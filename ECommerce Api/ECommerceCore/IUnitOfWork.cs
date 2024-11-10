using ECommerce.Core.IServices;
using ECommerce.Core.Models;

namespace ECommerce.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Category> Categories { get; }

        IProductRepository Products { get; }
        IAuthRepository Users { get; }

        int Complete();
    }
}
