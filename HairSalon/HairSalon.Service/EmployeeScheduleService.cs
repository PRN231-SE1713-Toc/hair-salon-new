using AutoMapper;
using HairSalon.Core;
using HairSalon.Core.Commons;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace HairSalon.Service
{
    public class EmployeeScheduleService(IUnitOfWork unitOfWork,
        IMapper mapper) : IEmployeeScheduleService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IMapper _mapper = mapper;
        // DELETE
        public async Task<ApiResponseModel<bool>> DeleteSchedule(EmployeeSchedule employeeSchedule)
        {
            try
            {
                if (employeeSchedule is not null)
                {
                    _unitOfWork.EmployeeScheduleRepository.Delete(employeeSchedule);
                    await _unitOfWork.CommitAsync();

                    return new ApiResponseModel<bool>
                    {
                        StatusCode = HttpStatusCode.OK,
                        Message = "Schedule deleted successfully.",
                        Response = true
                    };
                }

                return new ApiResponseModel<bool>
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Schedule not found.",
                    Response = false
                };
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();

                return new ApiResponseModel<bool>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"An error occurred: {ex.Message}",
                    Response = false
                };
            }
        }

        // GET ALL
        public async Task<ApiResponseModel<List<EmployeeScheduleResponse>>> GetSchedule()
        {
            try
            {
                var schedule = await _unitOfWork.EmployeeScheduleRepository.GetAll()
                    .Include(es => es.Employee)
                    .AsNoTracking()
                    .ToListAsync();

                var response = _mapper.Map<List<EmployeeScheduleResponse>>(schedule);

                return new ApiResponseModel<List<EmployeeScheduleResponse>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Schedules retrieved successfully.",
                    Response = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<EmployeeScheduleResponse>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"An error occurred: {ex.Message}",
                    Response = null
                };
            }
        }

        // GET By ID
        public async Task<ApiResponseModel<EmployeeScheduleResponse?>> GetSchedule(int id)
        {
            try
            {
                var schedule = await _unitOfWork.EmployeeScheduleRepository.GetAll()
                    .Include(es => es.Employee)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (schedule == null)
                {
                    return new ApiResponseModel<EmployeeScheduleResponse?>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Schedule not found.",
                        Response = null
                    };
                }

                var response = _mapper.Map<EmployeeScheduleResponse>(schedule);

                return new ApiResponseModel<EmployeeScheduleResponse?>
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Schedule retrieved successfully.",
                    Response = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<EmployeeScheduleResponse?>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"An error occurred: {ex.Message}",
                    Response = null
                };
            }
        }

        public async Task<EmployeeSchedule?> GetScheduleEntityById(int id)
        {
            return await _unitOfWork.EmployeeScheduleRepository.GetAsync(es => es.Id == id);
        }

        // UPDATE
        public async Task<ApiResponseModel<EmployeeScheduleResponse?>> UpdateSchedule(int id, UpdateEmployeeSchedule updatedSchedule)
        {
            try
            {
                var schedule = await _unitOfWork.EmployeeScheduleRepository.FindByIdAsync(id);
                if (schedule == null)
                {
                    return new ApiResponseModel<EmployeeScheduleResponse?>
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        Message = "Schedule not found.",
                        Response = null
                    };
                }

                _mapper.Map(updatedSchedule, schedule);
                _unitOfWork.EmployeeScheduleRepository.Update(schedule);
                await _unitOfWork.CommitAsync();

                var response = _mapper.Map<EmployeeScheduleResponse>(schedule);

                return new ApiResponseModel<EmployeeScheduleResponse?>
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Schedule updated successfully.",
                    Response = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<EmployeeScheduleResponse?>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"An error occurred: {ex.Message}",
                    Response = null
                };
            }
        }

        public async Task<ApiResponseModel<List<EmployeeScheduleResponse>>> GetScheduleOfEmployee(int empId)
        {
            try
            {
                var schedules = await _unitOfWork.EmployeeScheduleRepository.GetAll()
                    .AsNoTracking()
                    .Include(es => es.Employee)
                    .Where(es => es.EmployeeId == empId).ToListAsync();

                var response = _mapper.Map<List<EmployeeScheduleResponse>>(schedules);

                return new ApiResponseModel<List<EmployeeScheduleResponse>>
                {
                    StatusCode = HttpStatusCode.OK,
                    Message = "Schedules retrieved successfully.",
                    Response = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<List<EmployeeScheduleResponse>>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"An error occurred: {ex.Message}",
                    Response = null
                };
            }
        }

        public async Task<ApiResponseModel<EmployeeScheduleResponse?>> CreateSchedule(CreateEmployeeScheduleModel createScheduleDto)
        {
            try
            {
                var employeeExists = await _unitOfWork.EmployeeRepository.ExistsAsync(e => e.Id == createScheduleDto.EmployeeId);
                if (!employeeExists)
                {
                    return new ApiResponseModel<EmployeeScheduleResponse?>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Employee does not exist.",
                        Response = null
                    };
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
                    return new ApiResponseModel<EmployeeScheduleResponse?>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = $"Error parsing times: {ex.Message}",
                        Response = null
                    };
                }

                if (workingStartTime >= workingEndTime)
                {
                    return new ApiResponseModel<EmployeeScheduleResponse?>
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Invalid schedule times: Start time is after end time.",
                        Response = null
                    };
                }

                var employeeSchedule = _mapper.Map<EmployeeSchedule>(createScheduleDto);
                _unitOfWork.EmployeeScheduleRepository.Add(employeeSchedule);
                await _unitOfWork.CommitAsync();

                var response = _mapper.Map<EmployeeScheduleResponse>(employeeSchedule);

                return new ApiResponseModel<EmployeeScheduleResponse?>
                {
                    StatusCode = HttpStatusCode.Created,
                    Message = "Schedule created successfully.",
                    Response = response
                };
            }
            catch (Exception ex)
            {
                return new ApiResponseModel<EmployeeScheduleResponse?>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"An error occurred: {ex.Message}",
                    Response = null
                };
            }
        }

        public async Task<IList<StylistResponseModel>> GetAvailableStylists(DateOnly date, TimeOnly from, TimeOnly to)
        {
            var stylists = await _unitOfWork.EmployeeScheduleRepository.GetAll()
                .AsNoTracking()
                .Include(es => es.Employee)
                .Where(es => es.WorkingDate == date && es.WorkingStartTime >= from && es.WorkingEndTime <= to)
                .Select(es => es.Employee)
                .ToListAsync();
            return _mapper.Map<IList<StylistResponseModel>>(stylists);
        }

        public async Task<IList<StylistResponseModel>> GetAvailableStylists(DateOnly date, TimeOnly? from, TimeOnly? to)
        {
            if (from.HasValue && to.HasValue)
            {
                var stylists = await _unitOfWork.EmployeeScheduleRepository.GetAll()
                    .AsNoTracking()
                    .Include(es => es.Employee)
                    .Where(es => es.WorkingDate == date && es.WorkingStartTime >= from && es.WorkingEndTime <= to)
                    .Select(es => es.Employee)
                    .ToListAsync();
                return _mapper.Map<IList<StylistResponseModel>>(stylists);
            }
            else
            {
                var stylists = await _unitOfWork.EmployeeScheduleRepository.GetAll()
                    .AsNoTracking()
                    .Include(es => es.Employee)
                    .Where(es => es.WorkingDate != date)
                    .Select(es => es.Employee)
                    .ToListAsync();
                return _mapper.Map<IList<StylistResponseModel>>(stylists);
            }
        }
    }
}
