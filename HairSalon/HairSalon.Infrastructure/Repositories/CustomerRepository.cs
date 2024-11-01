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
        public async Task<Pagination<Customer>> GetCustomerByFilterAsync(PaginationParameter paginationParameter, CustomerFilterDTO customerFilterDTO)
        {
            try
            {
                var customersQuery = _dbContext.Customers.AsQueryable();
                customersQuery = await ApplyFilterSortAndSearch(customersQuery, customerFilterDTO);
                if (customersQuery != null)
                {
                    var customerQuery = ApplySorting(customersQuery, customerFilterDTO);
                    var totalCount = await customerQuery.CountAsync();

                    var customerPagination = await customerQuery
                        .Skip((paginationParameter.Page - 1) * paginationParameter.Limit)
                        .Take(paginationParameter.Limit)
                        .ToListAsync();
                    return new Pagination<Customer>(customerPagination, totalCount, paginationParameter.Page, paginationParameter.Limit);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IQueryable<Customer>> ApplyFilterSortAndSearch(IQueryable<Customer> Query, CustomerFilterDTO customerFilterDTO)
        {
            if (customerFilterDTO == null)
            {
                return Query;
            }
            if (!string.IsNullOrEmpty(customerFilterDTO.Search))
            {
                Query = Query.Where(x => x.Name.Contains(customerFilterDTO.Search));
            }
            if (!string.IsNullOrEmpty(customerFilterDTO.Search))
            {
                Query = Query.Where(x => x.Address.Contains(customerFilterDTO.Search));
            }
            if (!string.IsNullOrEmpty(customerFilterDTO.Search))
            {
                Query = Query.Where(x => x.Email.Contains(customerFilterDTO.Search));
            }
            if (!string.IsNullOrEmpty(customerFilterDTO.Search))
            {
                Query = Query.Where(x => x.PhoneNumber.Contains(customerFilterDTO.Search));
            }
            return Query;
        }
        private IQueryable<Customer> ApplySorting(IQueryable<Customer> query, CustomerFilterDTO customerFilterDTO)
        {
            switch (customerFilterDTO.Sort.ToLower())
            {
                case "customerName":
                    query = (customerFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.Name) : query.OrderBy(x => x.Name);
                    query = (customerFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id);
                    break;
                case "customerEmail":
                    query = (customerFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(x => x.Email) : query.OrderBy(x => x.Email);
                    break;
                default:
                    query = (customerFilterDTO.SortDirection.ToLower() == "desc") ? query.OrderByDescending(a => a.Id) : query.OrderBy(a => a.Id);
                    break;
            }
            return query;
        }
    }
}
