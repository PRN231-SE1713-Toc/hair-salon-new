using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core.Entities;

namespace HairSalon.Infrastructure.Repositories
{
    public class EmployeeScheduleRepository : Repository<EmployeeSchedule>, IEmployeeScheduleRepository
    {
        public EmployeeScheduleRepository(HairSalonDbContext dbContext) : base(dbContext)
        {
        }
    }
}
