using HairSalon.Core.Commons;
using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core.Dtos.PaginationDtos;
using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly HairSalonDbContext _dbContext;
        public CustomerRepository(HairSalonDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
