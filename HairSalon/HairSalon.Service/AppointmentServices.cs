using AutoMapper;
using HairSalon.Core;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore;

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

        public async Task<bool> CreateAppointment(AppointmentCreateModel newAppointment)
        {
            try {
                var appointment = _mapper.Map<Appointment>(newAppointment);

                //add Appointment
                _unitOfWork.AppointmentRepository.Add(appointment);

                //add AppointmentService
                var newAppointmentId = (await _unitOfWork.AppointmentRepository.GetAll().LastOrDefaultAsync()).Id;
                if (newAppointment.AppointmentServices != null && newAppointment.AppointmentServices.Any())
                {
                    foreach (var service in newAppointment.AppointmentServices)
                    {
                        AppointmentService appointmentService = new AppointmentService{
                            AppointmentId = newAppointmentId,
                            ServiceId = service.ServiceId,
                            CurrentPrice = service.CurrentPrice
                        };
                        _unitOfWork.AppointmentServiceRepository.Add(appointmentService);
                    }
                }
                else
                {
                    await _unitOfWork.RollbackAsync();
                    return false;
                }

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteAppointment(int id)
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
                    return false;

                // Delete AppointmentServices
                foreach (var service in appointment.AppointmentServices)
                {
                    _unitOfWork.AppointmentServiceRepository.Delete(service);
                }

                // Delete Appointment
                _unitOfWork.AppointmentRepository.Delete(appointment);

                await _unitOfWork.CommitAsync();
                return true;
            }
            catch {
                await _unitOfWork.RollbackAsync();
                return false;
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

        public async Task<bool> UpdateAppointment(AppointmentUpdateModel updatedAppointment)
        {
            try {
                var appointment = await _unitOfWork.AppointmentRepository.FindByIdAsync(updatedAppointment.Id);
                var appointmentServices = await _unitOfWork.AppointmentServiceRepository
                    .GetAll()
                    .Where(s => s.AppointmentId == updatedAppointment.Id)
                    .ToListAsync();
                //Update Appointment
                if (appointment == null) { return false; }
                _mapper.Map(updatedAppointment, appointment);
                _unitOfWork.AppointmentRepository.Update(appointment);
                //Update AppointmentService
                if (appointmentServices != null && appointmentServices.Any())
                {
                    foreach (var service in appointmentServices)
                    {
                        _unitOfWork.AppointmentServiceRepository.Update(service);
                    }
                }
                else
                {
                    return false;
                }
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch (Exception e) {
                await _unitOfWork.CommitAsync();
                return false;
            }
        }
    }
}
