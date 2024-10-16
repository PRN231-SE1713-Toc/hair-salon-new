using AutoMapper;
using HairSalon.Core;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Responses;
using System.Security.Claims;

namespace HairSalon.Service
{
    public class EmployeeService(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ITokenService tokenService) : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<LoginEmployeeResponse?> CheckLoginForEmployee(string email, string password)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetAsync(c => c.Email == email && c.Password == password);
            if (employee is null) return null;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.PrimarySid, employee.Id.ToString()),
                new Claim(ClaimTypes.Email, employee.Email),
                new Claim(ClaimTypes.Role, employee.Role.ToString())
            };
            var accessToken = _tokenService.GenerateAccessToken(claims);

            var response = _mapper.Map<LoginEmployeeResponse>(employee);
            response.AccessToken = accessToken;
            return response;
        }
    }
}
