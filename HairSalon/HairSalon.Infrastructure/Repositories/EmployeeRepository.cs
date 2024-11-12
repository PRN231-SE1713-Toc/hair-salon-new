using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HairSalon.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly HairSalonDbContext _context;
        public EmployeeRepository(HairSalonDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> ExistsAsync(Expression<Func<Employee, bool>> predicate)
        {
            return await _context.Set<Employee>().AnyAsync(predicate);
        }
    }
}
