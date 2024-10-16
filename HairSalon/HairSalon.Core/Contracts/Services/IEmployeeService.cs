using HairSalon.Core.Dtos.Responses;

namespace HairSalon.Core.Contracts.Services
{
    public interface IEmployeeService
    {
        Task<LoginEmployeeResponse?> CheckLoginForEmployee(string email, string password);  


    }
}
