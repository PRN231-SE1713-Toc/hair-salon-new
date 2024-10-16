using System.Linq.Expressions;

namespace HairSalon.Core
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();

        Task<T?> FindByIdAsync(params object[] keyValues);

        Task<T?> GetAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellation = default);

        void Add(T entity);

        void AddRange(IEnumerable<T> entities);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

        void Update(T entity);

        void UpdateRange(IEnumerable<T> entities);
    }
}
