using HairSalon.Core;
using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core.Entities;

namespace HairSalon.Infrastructure.Repositories
{
    public class AppointmentServiceRepository : Repository<AppointmentService>, IAppointmentServiceRepository
    {
        public AppointmentServiceRepository(HairSalonDbContext dbContext) : base(dbContext)
        {
        }
    }
}
