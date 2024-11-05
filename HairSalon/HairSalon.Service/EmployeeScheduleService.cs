using AutoMapper;
using HairSalon.Core;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Service
{
    public class EmployeeScheduleService(IUnitOfWork unitOfWork,
        IMapper mapper) : IEmployeeScheduleService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;


        // DELETE
        public async Task<bool> DeleteSchedule(EmployeeSchedule employeeSchedule)
        {
            try
            {
                if (employeeSchedule is not null)
                {
                    _unitOfWork.EmployeeScheduleRepository.Delete(employeeSchedule);
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


        // GET ALL
        public async Task<List<EmployeeScheduleResponse>> GetSchedule()
        {
            var schedule = await _unitOfWork.EmployeeScheduleRepository.GetAll()
                .AsNoTracking()
                .ToListAsync();
            return _mapper.Map<List<EmployeeScheduleResponse>>(schedule);
        }

        // GET By ID
        public async Task<EmployeeScheduleResponse?> GetSchedule(int id)
        {
            var schedule = await _unitOfWork.EmployeeScheduleRepository.GetAsync(c => c.Id == id);
            return _mapper.Map<EmployeeScheduleResponse>(schedule);

        }

        // UPDATE
        public async Task<bool> UpdateSchedule(UpdateEmployeeSchedule updatedSchedule)
        {
            try
            {
                var schedule = await _unitOfWork.EmployeeScheduleRepository.FindByIdAsync(updatedSchedule.Id);
                if (schedule is not null)
                {
                    _ = _mapper.Map(updatedSchedule, schedule);
                    _unitOfWork.EmployeeScheduleRepository.Update(schedule);
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

        public async Task<List<EmployeeScheduleResponse>> GetScheduleOfEmployee(int empId)
        {
            var schedules = await _unitOfWork.EmployeeScheduleRepository.GetAll()
                .AsNoTracking()
                .Include(es => es.Employee)
                .Where(es => es.EmployeeId == empId).ToListAsync();

            return _mapper.Map<List<EmployeeScheduleResponse>>(schedules);
        }
    }
}
