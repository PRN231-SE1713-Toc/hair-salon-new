using HairSalon.Core.Contracts.Repositories;
using HairSalon.Core;
using HairSalon.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HairSalon.Core.Dtos.Responses;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Service
{
    public class ServiceService : IServiceService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Core.Entities.Service> CreateService(Core.Entities.Service service)
        {
            try
            {
                if (service == null)
                    throw new ArgumentNullException(nameof(service));

                if (string.IsNullOrWhiteSpace(service.Name) || service.Name.Length > 100)
                    throw new ArgumentException("Service Name is required and cannot exceed 100 characters.");


                if (!string.IsNullOrEmpty(service.Description) && service.Description.Length > 500)
                    throw new ArgumentException("Description cannot exceed 500 characters.");

                if (!string.IsNullOrEmpty(service.Duration))
                {
                    var regex = new System.Text.RegularExpressions.Regex(@"^\d{1,3}:\d{2}$");
                    if (!regex.IsMatch(service.Duration))
                        throw new ArgumentException("Duration must be in the format of HH:mm.");
                }

                if (service.Price < 0)
                    throw new ArgumentException("Price must be greater than 0.");

                _unitOfWork.ServiceRepository.Add(service);
                await _unitOfWork.CommitAsync();

                return service;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                return null;
            }

        }

        public async Task<List<Core.Entities.Service>> GetServices()
        {
            var services = await _unitOfWork.ServiceRepository.GetAll().AsNoTracking().ToListAsync();
            return services;
        }

        public async Task<Core.Entities.Service> GetServicesById(int id)
        {
            return await _unitOfWork.ServiceRepository.FindByIdAsync(id);
        }
    }
}
