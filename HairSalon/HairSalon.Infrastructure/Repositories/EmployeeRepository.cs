using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core.Entities;

namespace HairSalon.Infrastructure.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(HairSalonDbContext dbContext) : base(dbContext)
        {
        }
    }
}
