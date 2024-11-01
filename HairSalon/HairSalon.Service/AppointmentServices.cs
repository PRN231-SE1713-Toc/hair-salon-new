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
                _unitOfWork.AppointmentRepository.Add(appointment);
                //Create AppointmentService
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
                var result = await _unitOfWork.AppointmentRepository
                .GetAll()
                .Where(a => a.Id == id)
                .Include(a => a.Customer)
                .Include(a => a.Stylist)
                .Include(a => a.AppointmentServices)
                .Include(a => a.Transactions)
                .AsNoTracking()
                .FirstOrDefaultAsync();
                //Delete AppointmentServices
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
            if (result == null) throw new Exception("not found!");
            AppointmentViewResponse appointmentViewResponse = new AppointmentViewResponse
            {
                Id = id,
                CustomerName = result.Customer.Name,
                StylistName = result.Stylist.Name,
                AppointmentDate = result.AppointmentDate,
                StartTime = result.StartTime,
                EndTime = result.EndTime,
                Note = result.Note,
                AppointmentStatus = result.AppointmentStatus,
                AppointmentServices = result.AppointmentServices,
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
            if (list == null) { throw new Exception("not found"); }
            var appointments = list.Select(result => new AppointmentViewResponse
            {
                CustomerName = result.Customer.Name,
                StylistName = result.Stylist.Name,
                AppointmentDate = result.AppointmentDate,
                StartTime = result.StartTime,
                EndTime = result.EndTime,
                Note = result.Note,
                AppointmentStatus = result.AppointmentStatus,
                AppointmentServices = result.AppointmentServices,
                AppointmentCost = result.AppointmentServices.Sum(a => a.CurrentPrice)
            }).ToList();
            return appointments;
        }

        public async Task<bool> UpdateAppointment(Appointment updatedAppointment)
        {
            try {
                var appointment = await _unitOfWork.AppointmentRepository.FindByIdAsync(updatedAppointment.Id);
                var appointmentServices = await _unitOfWork.AppointmentServiceRepository
                    .GetAll()
                    .Where(s => s.AppointmentId == updatedAppointment.Id)
                    .ToListAsync();
                if (appointment == null) { return false; }
                _mapper.Map(updatedAppointment, appointment);
                _unitOfWork.AppointmentRepository.Update(appointment);
                //Update AppointmentService
                //if (appointmentServices != null && appointmentServices.Any())
                //{
                //    foreach (var service in appointmentServices)
                //    {
                //        _unitOfWork.AppointmentServiceRepository.Update(service);  
                //    }
                //}
                //else
                //{
                //    return false;
                //}
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
