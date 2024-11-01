using HairSalon.Core.Commons;
using HairSalon.Core.Dtos.PaginationDtos;
using HairSalon.Core.Entities;

namespace HairSalon.Core.Contracts.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        public Task<Pagination<Customer>> GetCustomerByFilterAsync(PaginationParameter paginationParameter, CustomerFilterDTO customerFilterDTO);
    }
}
