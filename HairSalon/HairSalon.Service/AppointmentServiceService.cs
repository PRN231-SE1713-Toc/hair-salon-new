using HairSalon.Core;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Service
{
    public class AppointmentServiceService : IAppointmentServiceService
    {

        private readonly IUnitOfWork _unitOfWork;
        public AppointmentServiceService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<AppointmentService> CreateServiceAsync(CreateAppointmentServiceModel model)
        {
            try
            {
                var _serviceRepo = _unitOfWork.ServiceRepository;
                var _appoinementRepo = _unitOfWork.AppointmentRepository;

                var serviceExist = await _serviceRepo.FindByIdAsync(model.ServiceId);
                var appointmentExist = await _appoinementRepo.FindByIdAsync(model.AppointmentId);

                if(serviceExist == null)
                {
                    throw new ArgumentException("Service not found");
                }                
                
                if(appointmentExist == null)
                {
                    throw new ArgumentException("Appoinment not found");
                }

                var newService = new AppointmentService
                {
                    AppointmentId = model.AppointmentId,
                    ServiceId = model.ServiceId,
                    CurrentPrice = model.CurrentPrice
                };

                _unitOfWork.AppointmentServiceRepository.Add(newService);
                await _unitOfWork.CommitAsync();

                return newService;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }

        }

        public Task DeleteServiceAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AppointmentService>> GetAllServicesAsync()
        {
            return await _unitOfWork.AppointmentServiceRepository.GetAll().ToListAsync();
        }

        public async Task<AppointmentService> GetServiceByIdAsync(int id)
        {
            return await _unitOfWork.AppointmentServiceRepository.FindByIdAsync(id);
        }

        public async Task<AppointmentService> UpdateServiceAsync(int id, UpdateAppointmentServiceModel model)
        {
            try
            {
                var existingService = await _unitOfWork.AppointmentServiceRepository.FindByIdAsync(id);
                if (existingService == null)
                {
                    throw new ArgumentException("Appointment not found.");
                }

                var _serviceRepo = _unitOfWork.ServiceRepository;
                var _appoinementRepo = _unitOfWork.AppointmentRepository;

                var serviceExist = await _serviceRepo.FindByIdAsync(model.ServiceId);
                var appointmentExist = await _appoinementRepo.FindByIdAsync(model.AppointmentId);

                if (serviceExist == null)
                {
                    throw new ArgumentException("Service not found");
                }

                if (appointmentExist == null)
                {
                    throw new ArgumentException("Appoinment not found");
                }

                // Update fields
                existingService.AppointmentId = model.AppointmentId;
                existingService.ServiceId = model.ServiceId;
                existingService.CurrentPrice = model.CurrentPrice;

                // Perform validation
                ValidateAppointmentService(existingService);

                // Update in the repository
                _unitOfWork.AppointmentServiceRepository.Update(existingService);
                await _unitOfWork.CommitAsync();

                return existingService;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void ValidateAppointmentService(Core.Entities.AppointmentService appointmentService)
        {
            if (appointmentService.AppointmentId <= 0)
                throw new ArgumentException("Appointment ID is required and must be a positive number.");

            if (appointmentService.ServiceId <= 0)
                throw new ArgumentException("Service ID is required and must be a positive number.");

            if (appointmentService.CurrentPrice < 0)
                throw new ArgumentException("Current price must be greater than or equal to 0.");
        }
    }
}
