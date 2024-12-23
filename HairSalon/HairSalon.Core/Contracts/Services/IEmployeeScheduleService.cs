﻿using HairSalon.Core.Commons;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;

namespace HairSalon.Core.Contracts.Services
{
    public interface IEmployeeScheduleService
    {
        Task<ApiResponseModel<List<EmployeeScheduleResponse>>> GetSchedule();
        Task<ApiResponseModel<EmployeeScheduleResponse?>> GetSchedule(int id);
        Task<EmployeeSchedule?> GetScheduleEntityById(int id);
        Task<ApiResponseModel<EmployeeScheduleResponse?>> UpdateSchedule(int id, UpdateEmployeeSchedule updatedSchedule);
        Task<ApiResponseModel<bool>> DeleteSchedule(EmployeeSchedule employeeSchedule);
        Task<ApiResponseModel<List<EmployeeScheduleResponse>>> GetScheduleOfEmployee(int empId);
        Task<ApiResponseModel<EmployeeScheduleResponse?>> CreateSchedule(CreateEmployeeScheduleModel createScheduleDto);

        Task<IList<StylistResponseModel>> GetAvailableStylists(DateOnly date, TimeOnly? from, TimeOnly? to);
    }
}
