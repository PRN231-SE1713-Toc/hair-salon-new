using HairSalon.Core.Commons;
using HairSalon.Core.Dtos.PaginationDtos;
using HairSalon.Core.Entities;

namespace HairSalon.Core.Contracts.Repositories
{
    public interface IServiceRepository : IRepository<Service>
    {
        public Task<Pagination<Service>> GetServiceByFilterAsync(PaginationParameter paginationParameter, ServiceFilterDTO serviceFilterDTO);
    }
}
