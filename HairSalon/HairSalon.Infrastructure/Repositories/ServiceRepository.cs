using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core.Entities;

namespace HairSalon.Infrastructure.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(HairSalonDbContext dbContext) : base(dbContext)
        {
        }
    }
}
