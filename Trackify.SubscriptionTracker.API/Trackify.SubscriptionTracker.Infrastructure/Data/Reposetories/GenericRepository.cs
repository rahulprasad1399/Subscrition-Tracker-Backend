using Microsoft.EntityFrameworkCore;
using Trackify.SubscriptionTracker.Application.Interface;

namespace Trackify.SubscriptionTracker.Infrastructure.Data.Reposetories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly IApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public GenericRepository(IApplicationDbContext context, CancellationToken cancellationToken = default) 
        {
            _context = context;
            _dbSet = _context.Set<T>();

        }
        public async Task<int> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        

        public async Task<int> DeleteRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            _dbSet.RemoveRange(entities);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity =await _dbSet.FindAsync(id, cancellationToken);
            if (entity == null) return 0;

            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id, cancellationToken);
        }

        public async Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FindAsync(id, cancellationToken);
            if (entity != null) 
            {
                _context.Entry(entity).State = EntityState.Detached;
                return true;
            } 
            return false;
        }
    }
}
