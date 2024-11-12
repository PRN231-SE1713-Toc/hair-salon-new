using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Infrastructure.Repositories
{
    public class EmployeeScheduleRepository : Repository<EmployeeSchedule>, IEmployeeScheduleRepository
    {
        public EmployeeScheduleRepository(HairSalonDbContext dbContext) : base(dbContext)
        {
        }
    }
}
