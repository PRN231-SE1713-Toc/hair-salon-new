using HairSalon.Core.Entities;
using System.Linq.Expressions;

namespace HairSalon.Core.Contracts.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<bool> ExistsAsync(Expression<Func<Employee, bool>> predicate);
    }
}
