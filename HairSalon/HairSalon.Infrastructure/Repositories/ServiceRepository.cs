using HairSalon.Core.Commons;
using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core.Dtos.PaginationDtos;
using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Infrastructure.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        private readonly HairSalonDbContext _dbContext;
        public ServiceRepository(HairSalonDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Pagination<Service>> GetServiceByFilterAsync(PaginationParameter paginationParameter, ServiceFilterDTO serviceFilterDTO)
        {
            try
            {
                var ServicesQuery = _dbContext.Services.AsQueryable();
                ServicesQuery = await ApplyFilterSortAndSearch(ServicesQuery, serviceFilterDTO);
                if (ServicesQuery != null)
                {
                    var serviceQuery = ApplySorting(ServicesQuery, serviceFilterDTO);
                    var totalCount = await serviceQuery.CountAsync();

                    var servicePagination = await serviceQuery
                        .Skip((paginationParameter.Page - 1) * paginationParameter.Limit)
                        .Take(paginationParameter.Limit)
                        .ToListAsync();
                    return new Pagination<Service>(servicePagination, totalCount, paginationParameter.Page, paginationParameter.Limit);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IQueryable<Service>> ApplyFilterSortAndSearch(IQueryable<Service> Query, ServiceFilterDTO serviceFilterDTO)
        {
            if (serviceFilterDTO == null)
            {
                return Query;
            }
            if (!string.IsNullOrEmpty(serviceFilterDTO.Search))
            {
                Query = Query.Where(x => x.Name.Equals(serviceFilterDTO.Search));
            }
            return Query;
        }
        private IQueryable<Service> ApplySorting(IQueryable<Service> query, ServiceFilterDTO serviceFilterDTO)
        {
            switch (serviceFilterDTO.Sort.ToLower())
            {
                case "serviceName":
                    query = (serviceFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                    break;
                default:
                    query = (serviceFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id);
                    break;
            }
            return query;
        }
    }
}
