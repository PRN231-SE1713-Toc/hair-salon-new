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

        public async Task<EmployeeScheduleResponse> CreateSchedule(CreateEmployeeScheduleModel createScheduleDto)
        {
            try
            {
                Console.WriteLine($"Received schedule request: EmployeeId = {createScheduleDto.EmployeeId}, Start Time = {createScheduleDto.WorkingStartTime}, End Time = {createScheduleDto.WorkingEndTime}");

                var employeeExists = await _unitOfWork.EmployeeRepository.ExistsAsync(e => e.Id == createScheduleDto.EmployeeId);
                if (!employeeExists)
                {
                    Console.WriteLine("Employee does not exist with the provided ID.");
                    return null;
                }

                TimeOnly workingStartTime;
                TimeOnly workingEndTime;

                try
                {
                    workingStartTime = TimeOnly.Parse(createScheduleDto.WorkingStartTime);
                    workingEndTime = TimeOnly.Parse(createScheduleDto.WorkingEndTime);
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error parsing times: {ex.Message}");
                    return null;
                }

                Console.WriteLine($"Parsed start time: {workingStartTime}, Parsed end time: {workingEndTime}");

                if (workingStartTime >= workingEndTime)
                {
                    Console.WriteLine("Invalid schedule times: Start time is after end time.");
                    return null;
                }

                var employeeSchedule = _mapper.Map<EmployeeSchedule>(createScheduleDto);

                Console.WriteLine($"Mapped EmployeeSchedule: {employeeSchedule.Id}, Start Time = {employeeSchedule.WorkingStartTime}, End Time = {employeeSchedule.WorkingEndTime}");

                _unitOfWork.EmployeeScheduleRepository.Add(employeeSchedule);
                await _unitOfWork.CommitAsync();

                Console.WriteLine("Schedule successfully created and committed to the database.");

                return _mapper.Map<EmployeeScheduleResponse>(employeeSchedule);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message} \nStackTrace: {ex.StackTrace}");

                await _unitOfWork.RollbackAsync();

                return null;
            }
        }


    }
}
