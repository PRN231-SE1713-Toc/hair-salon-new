using AutoMapper;
using HairSalon.Core;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HairSalon.Service
{
    public class CustomerService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ITokenService tokenService) : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<LoginCustomerResponse?> CheckLoginForCustomer(string email, string password)
        {
            var customer = await _unitOfWork.CustomerRepository.GetAsync(c => c.Email == email && c.Password == password);
            if (customer is null) return null;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.PrimarySid, customer.Id.ToString()),
                new Claim(ClaimTypes.Email, customer.Email),
                new Claim(ClaimTypes.Role, "Customer")
            };
            var accessToken = _tokenService.GenerateAccessToken(claims);
            
            var response = _mapper.Map<LoginCustomerResponse>(customer);
            response.AccessToken = accessToken;
            return response;
        }

        public Task<bool> CreateCustomer()
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCustomer()
        {
            throw new NotImplementedException();
        }

        public Task<Customer?> GetCustomerById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Customer>> GetCustomers()
        {
            return await _unitOfWork.CustomerRepository.GetAll().AsNoTracking().ToListAsync();
        }

        public Task<bool> UpdateCustomer()
        {
            throw new NotImplementedException();
        }
    }
}
