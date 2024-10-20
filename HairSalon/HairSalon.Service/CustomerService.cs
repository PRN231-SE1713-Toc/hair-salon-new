﻿using AutoMapper;
using HairSalon.Core;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
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

        public async Task<bool> CreateCustomer(CreatedCustomerModel request)
        {
            try
            {
                var customer = _mapper.Map<Customer>(request);
                _unitOfWork.CustomerRepository.Add(customer);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteCustomer(Customer customer)
        {
            try
            {
                if (customer is not null)
                {
                    _unitOfWork.CustomerRepository.Delete(customer);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<Customer?> GetCustomerById(int id) => await _unitOfWork.CustomerRepository.FindByIdAsync(id);

        public async Task<bool> UpdateCustomer(UpdatedCustomer updatedCustomer)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.FindByIdAsync(updatedCustomer.Id);
                if (customer is not null)
                {
                    _ = _mapper.Map(updatedCustomer, customer);
                    _unitOfWork.CustomerRepository.Update(customer);
                    await _unitOfWork.CommitAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                await _unitOfWork.CommitAsync();
                return false;
            }
        }

        public async Task<List<CustomerResponse>> GetCustomers()
        {
            var customers = await _unitOfWork.CustomerRepository.GetAll()
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<CustomerResponse>>(customers);
        }

        public async Task<CustomerResponse> GetCustomer(int id)
        {
            var customer = await _unitOfWork.CustomerRepository.GetAsync(c => c.Id == id);
            return _mapper.Map<CustomerResponse>(customer);
        }
    }
}
