using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;

namespace HairSalon.Core.Contracts.Services
{
    public interface ICustomerService
    {
        Task<LoginCustomerResponse?> CheckLoginForCustomer(string email, string password);

        Task<List<CustomerResponse>> GetCustomers();

        Task<Customer?> GetCustomerById(int id);

        Task<CustomerResponse> GetCustomer(int id);

        Task<bool> CreateCustomer(CreatedCustomerModel request);

        Task<bool> UpdateCustomer(UpdatedCustomer updatedCustomer);

        Task<bool> DeleteCustomer(Customer customer);
    }
}
