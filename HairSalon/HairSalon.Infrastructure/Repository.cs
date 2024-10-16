using HairSalon.Core;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HairSalon.Infrastructure
{
    public class Repository<T>(HairSalonDbContext dbContext) : IRepository<T> where T : class
    {
        private readonly HairSalonDbContext _dbContext = dbContext;
        private readonly DbSet<T> _dbSet = dbContext.Set<T>();

        public void Add(T entity) => _dbContext.Add(entity);

        public void AddRange(IEnumerable<T> entities) => _dbContext.AddRange(entities);

        public void Delete(T entity) => _dbContext.Remove(entity);

        public void DeleteRange(IEnumerable<T> entities) => _dbContext.RemoveRange(entities);

        public async Task<T?> FindByIdAsync(params object[] keyValues) => await _dbSet.FindAsync(keyValues);

        public IQueryable<T> GetAll() => _dbSet.AsQueryable();

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation = default)
            => await _dbSet.FirstOrDefaultAsync(predicate, cancellation);

        public void Update(T entity) => _dbContext.Update(entity);

        public void UpdateRange(IEnumerable<T> entities) => _dbContext.UpdateRange(entities);
    }
}
