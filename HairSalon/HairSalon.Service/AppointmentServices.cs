using AutoMapper;
using HairSalon.Core;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;
using HairSalon.Core.Enums;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HairSalon.Service
{
    public class AppointmentServices(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ITokenService tokenService) : IAppointmentServices
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        private readonly ITokenService _tokenService = tokenService;

        public async Task<string> CreateAppointment(AppointmentCreateModel newAppointment)
        {
            try {
                //check valid
                if (newAppointment == null) 
                    return "new appointment must not be empty";
                if (newAppointment.CustomerId <= 0) 
                    return "Customer Id must be greater than 0";
                if (newAppointment.StylistId <= 0) 
                    return "Stylist Id must be grater than 0";
                if (newAppointment.AppointmentDate < DateOnly.FromDateTime(DateTime.Now)) 
                    return "Booking date must be in the future";
                if (newAppointment.EndTime <= newAppointment.StartTime) 
                    return "EndTime must not before StartTime";
                if (newAppointment.AppointmentStatus < 0 ||
                    (int)newAppointment.AppointmentStatus > Enum.GetValues(typeof(AppointmentStatus)).Length)
                    return "Appointment status must equal or greater than 0 and less than " 
                        + Enum.GetValues(typeof(AppointmentStatus)).Length;
                if (newAppointment.AppointmentServices.Count == 0)
                    return "Service list must not be empty";
                var Services = new List<AppointmentService>();
                foreach (var appointmentService in newAppointment.AppointmentServices)
                {
                    var service = await _unitOfWork.ServiceRepository.FindByIdAsync(appointmentService.ServiceId);
                    if (service == null) return "service id " + appointmentService.ServiceId + " is not found";
                    Services.Add(new AppointmentService()
                    {
                        ServiceId = service.Id,
                        CurrentPrice = service.Price,
                    });
                }

                //add Appointment
                Appointment appointment = new Appointment()
                {
                    CustomerId = newAppointment.CustomerId,
                    StylistId = newAppointment.StylistId,
                    AppointmentDate = newAppointment.AppointmentDate,
                    StartTime = newAppointment.StartTime,
                    EndTime = newAppointment.EndTime,
                    Note = newAppointment.Note,
                    AppointmentStatus = newAppointment.AppointmentStatus,
                };
                _unitOfWork.AppointmentRepository.Add(appointment);

                await _unitOfWork.CommitAsync();

                //add AppointmentService
                foreach (var service in Services)
                {
                    service.AppointmentId = appointment.Id;
                    _unitOfWork.AppointmentServiceRepository.Add(service);
                }

                await _unitOfWork.CommitAsync();
                return "new appointment added successfully";
            }
            catch (Exception e) {
                await _unitOfWork.RollbackAsync();
                return e.Message;
            }
        }

        public async Task<string> DeleteAppointment(int id)
        {
            try {
                var appointment = await _unitOfWork.AppointmentRepository
                .GetAll()
                .Where(a => a.Id == id)
                .Include(a => a.Customer)
                .Include(a => a.Stylist)
                .Include(a => a.AppointmentServices)
                .Include(a => a.Transactions)
                .AsNoTracking()
                .FirstOrDefaultAsync();

                if (appointment == null)
                    return "Can not find appointment with id " + id;

                // Delete AppointmentServices
                foreach (var service in appointment.AppointmentServices)
                {
                    _unitOfWork.AppointmentServiceRepository.Delete(service);
                }

                // Delete Appointment
                _unitOfWork.AppointmentRepository.Delete(appointment);

                await _unitOfWork.CommitAsync();
                return "Deleted successfully";
            }
            catch (Exception e) {
                await _unitOfWork.RollbackAsync();
                return e.Message;
            }
        }

        public async Task<AppointmentViewResponse?> GetAppointment(int id)
        {
            var result = await _unitOfWork.AppointmentRepository
                .GetAll()
                .Where(a => a.Id == id)
                .Include(a => a.Customer)
                .Include(a => a.Stylist)
                .Include(a => a.AppointmentServices)
                .Include(a => a.Transactions)
                .AsNoTracking()
                .FirstOrDefaultAsync();

            //Not found
            if (result == null) return null;

            var appointmentServices = await Task.WhenAll(result.AppointmentServices.Select(async res =>
            {
                var service = await _unitOfWork.ServiceRepository.FindByIdAsync(res.ServiceId);
                return new AppointmentServiceDto
                {
                    Id = res.Id,
                    AppointmentId = res.AppointmentId,
                    ServiceId = res.ServiceId,
                    ServiceName = service?.Name ?? null,
                    CurrentPrice = res.CurrentPrice
                };
            }));
            AppointmentViewResponse appointmentViewResponse = new AppointmentViewResponse
            {
                Id = id,
                CustomerId = result.CustomerId,
                CustomerName = result.Customer.Name,
                StylistId = result.StylistId,
                StylistName = result.Stylist.Name,
                AppointmentDate = result.AppointmentDate,
                StartTime = result.StartTime,
                EndTime = result.EndTime,
                Note = result.Note,
                AppointmentStatus = result.AppointmentStatus.ToString(),
                AppointmentServices = appointmentServices,
                AppointmentCost = result.AppointmentServices.Sum(a => a.CurrentPrice)
            };
            return appointmentViewResponse;
        }
        public async Task<List<AppointmentViewResponse>> GetAppointments()
        {
            var list = await _unitOfWork.AppointmentRepository
                .GetAll()
                .Include(a => a.Customer)
                .Include(a => a.Stylist)
                .Include (a => a.AppointmentServices)
                .AsNoTracking()
                .ToListAsync();

            //No record
            if (list == null) return new List<AppointmentViewResponse>();

            var services = await _unitOfWork.ServiceRepository.GetAll().ToListAsync();

            var appointments = list.Select(result => new AppointmentViewResponse
            {
                Id = result.Id,
                CustomerId = result.CustomerId,
                CustomerName = result.Customer.Name,
                StylistId = result.StylistId,
                StylistName = result.Stylist.Name,
                AppointmentDate = result.AppointmentDate,
                StartTime = result.StartTime,
                EndTime = result.EndTime,
                Note = result.Note,
                AppointmentStatus = result.AppointmentStatus.ToString(),
                AppointmentServices = result.AppointmentServices.Select(res => new AppointmentServiceDto
                {
                    Id = res.Id,
                    AppointmentId = res.AppointmentId,
                    ServiceId = res.ServiceId,
                    ServiceName = services.FirstOrDefault(s => s.Id == res.ServiceId)?.Name ?? null,
                    CurrentPrice = res.CurrentPrice,
                }),
                AppointmentCost = result.AppointmentServices.Sum(a => a.CurrentPrice)
            }).ToList();

            return appointments;
        }

        public async Task<List<AppointmentViewResponse>> GetAppointmentsbyCustomerId(int customerId, int status)
        {
            List<Appointment> list = new List<Appointment>();
            if (status < 0) {
                list = await _unitOfWork.AppointmentRepository
                .GetAll()
                .Where(s => s.CustomerId == customerId)
                .Include(a => a.Customer)
                .Include(a => a.Stylist)
                .Include(a => a.AppointmentServices)
                .AsNoTracking()
                .ToListAsync();
            } else
            {
                list = await _unitOfWork.AppointmentRepository
                .GetAll()
                .Where(a => a.CustomerId == customerId && a.AppointmentStatus == (AppointmentStatus) status)
                .Include(a => a.Customer)
                .Include(a => a.Stylist)
                .Include(a => a.AppointmentServices)
                .AsNoTracking()
                .ToListAsync();
            }

            //No record
            if (list == null) return new List<AppointmentViewResponse>();

            var services = await _unitOfWork.ServiceRepository.GetAll().ToListAsync();

            var appointments = list.Select(result => new AppointmentViewResponse
            {
                Id = result.Id,
                CustomerId = result.CustomerId,
                CustomerName = result.Customer.Name,
                StylistId = result.StylistId,
                StylistName = result.Stylist.Name,
                AppointmentDate = result.AppointmentDate,
                StartTime = result.StartTime,
                EndTime = result.EndTime,
                Note = result.Note,
                AppointmentStatus = result.AppointmentStatus.ToString(),
                AppointmentServices = result.AppointmentServices.Select(res => new AppointmentServiceDto
                {
                    Id = res.Id,
                    AppointmentId = res.AppointmentId,
                    ServiceId = res.ServiceId,
                    ServiceName = services.FirstOrDefault(s => s.Id == res.ServiceId)?.Name ?? null,
                    CurrentPrice = res.CurrentPrice,
                }),
                AppointmentCost = result.AppointmentServices.Sum(a => a.CurrentPrice)
            }).ToList();

            return appointments;
        }

        public async Task<List<AppointmentViewResponse>> GetAppointmentsByStylistId(int stylistId, int status)
        {
            List<Appointment> list = new List<Appointment>();
            if (status < 0)
            {
                list = await _unitOfWork.AppointmentRepository
                .GetAll()
                .Where(s => s.CustomerId == stylistId)
                .Include(a => a.Customer)
                .Include(a => a.Stylist)
                .Include(a => a.AppointmentServices)
                .AsNoTracking()
                .ToListAsync();
            }
            else
            {
                list = await _unitOfWork.AppointmentRepository
                .GetAll()
                .Where(a => a.CustomerId == stylistId && a.AppointmentStatus == (AppointmentStatus) status)
                .Include(a => a.Customer)
                .Include(a => a.Stylist)
                .Include(a => a.AppointmentServices)
                .AsNoTracking()
                .ToListAsync();
            }

            //No record
            if (list == null) return new List<AppointmentViewResponse>();

            var services = await _unitOfWork.ServiceRepository.GetAll().ToListAsync();

            var appointments = list.Select(result => new AppointmentViewResponse
            {
                Id = result.Id,
                CustomerId = result.CustomerId,
                CustomerName = result.Customer.Name,
                StylistId = result.StylistId,
                StylistName = result.Stylist.Name,
                AppointmentDate = result.AppointmentDate,
                StartTime = result.StartTime,
                EndTime = result.EndTime,
                Note = result.Note,
                AppointmentStatus = result.AppointmentStatus.ToString(),
                AppointmentServices = result.AppointmentServices.Select(res => new AppointmentServiceDto
                {
                    Id = res.Id,
                    AppointmentId = res.AppointmentId,
                    ServiceId = res.ServiceId,
                    ServiceName = services.FirstOrDefault(s => s.Id == res.ServiceId)?.Name ?? null,
                    CurrentPrice = res.CurrentPrice,
                }),
                AppointmentCost = result.AppointmentServices.Sum(a => a.CurrentPrice)
            }).ToList();

            return appointments;
        }

        public async Task<string> UpdateAppointment(AppointmentUpdateModel updatedAppointment)
        {
            try {
                //check valid
                if (updatedAppointment.Id <= 0)
                    return "Id must greater than 0";
                Appointment appointment = await _unitOfWork.AppointmentRepository.FindByIdAsync(updatedAppointment.Id);
                if (appointment == null) 
                    return "Can not find Appointment with id " + updatedAppointment.Id;
                if (updatedAppointment == null)
                    return "new appointment must not be empty";
                if (updatedAppointment.CustomerId <= 0)
                    return "Customer Id must be greater than 0";
                if (updatedAppointment.StylistId <= 0)
                    return "Stylist Id must be grater than 0";
                if (updatedAppointment.AppointmentDate <= DateOnly.FromDateTime(DateTime.Now))
                    return "Booking date must be in the future";
                if (updatedAppointment.EndTime <= updatedAppointment.StartTime)
                    return "EndTime must not before StartTime";
                if (updatedAppointment.AppointmentStatus < 0 ||
                    (int) updatedAppointment.AppointmentStatus > Enum.GetValues(typeof(AppointmentStatus)).Length)
                    return "Appointment status must equal or greater than 0 and less than "
                        + Enum.GetValues(typeof(AppointmentStatus)).Length;
                if (updatedAppointment.AppointmentServices.Count == 0)
                    return "Service list must not be empty";
                var Services = new List<AppointmentService>();
                foreach (var appointmentService in updatedAppointment.AppointmentServices)
                {
                    var service = await _unitOfWork.ServiceRepository.FindByIdAsync(appointmentService.ServiceId);
                    if (service == null) return "service id " + appointmentService.ServiceId + " is not found";
                    Services.Add(new AppointmentService()
                    {
                        ServiceId = service.Id,
                        CurrentPrice = appointmentService.CurrentPrice,
                    });
                }

                //update Appointment
                appointment = new Appointment()
                {
                    Id = updatedAppointment.Id,
                    CustomerId = updatedAppointment.CustomerId,
                    StylistId = updatedAppointment.StylistId,
                    AppointmentDate = updatedAppointment.AppointmentDate,
                    StartTime = updatedAppointment.StartTime,
                    EndTime = updatedAppointment.EndTime,
                    Note = updatedAppointment.Note,
                    AppointmentStatus = updatedAppointment.AppointmentStatus,
                };
                _unitOfWork.AppointmentRepository.Update(appointment);

                //add AppointmentService
                foreach (var service in Services)
                {
                    _unitOfWork.AppointmentServiceRepository.Update(service);
                }

                await _unitOfWork.CommitAsync();
                return "Appointment updated successfully";
            }
            catch (Exception e)
            {
                await _unitOfWork.RollbackAsync();
                return e.Message;
            }
        }
    }
}
