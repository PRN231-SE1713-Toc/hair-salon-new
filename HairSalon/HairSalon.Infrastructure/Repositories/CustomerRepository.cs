using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core.Entities;

namespace HairSalon.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(HairSalonDbContext dbContext) : base(dbContext)
        {
        }
    }
}
