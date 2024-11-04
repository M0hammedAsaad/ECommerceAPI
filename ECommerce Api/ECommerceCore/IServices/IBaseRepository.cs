using System.Linq.Expressions;

namespace ECommerce.Core.IServices
{
    public interface IBaseRepository<T> where T : class
    {
        Task AddAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>> orderBy = null);
        Task<T> GetByIdAsync(int id);
        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);
        Task<T> FindByNameAsync(Expression<Func<T, bool>> criteria);
    }
}
