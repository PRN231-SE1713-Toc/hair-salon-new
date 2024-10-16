using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;

namespace HairSalon.Core.Contracts.Services
{
    public interface ICustomerService
    {
        Task<LoginCustomerResponse?> CheckLoginForCustomer(string email, string password);

        Task<List<Customer>> GetCustomers();



        Task<Customer?> GetCustomerById(int id);

        Task<bool> CreateCustomer();

        Task<bool> UpdateCustomer();

        Task<bool> DeleteCustomer();
    }
}
