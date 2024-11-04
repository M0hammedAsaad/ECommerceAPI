using ECommerce.Core.IServices;
using ECommerce.InfraStructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ECommerce.InfraStructure.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T entity)
        {
            await _context.AddRangeAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var record = await GetByIdAsync(id);
            _context.Set<T>().Remove(record);
            await _context.SaveChangesAsync();
        }

        public async Task<T> FindByNameAsync(Expression<Func<T, bool>> criteria)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(criteria);
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, object>> orderBy)
        {

            return await _context.Set<T>().OrderBy(orderBy).ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
