using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Contracts.Services
{
    public interface IEmployeeScheduleService
    {
        Task<List<EmployeeScheduleResponse>> GetSchedule();
        Task<EmployeeScheduleResponse?> GetSchedule(int id);
        Task<bool> UpdateSchedule(UpdateEmployeeSchedule updatedSchedule);
        Task<bool> DeleteSchedule(EmployeeSchedule employeeSchedule);

        Task<List<EmployeeScheduleResponse>> GetScheduleOfEmployee(int empId);
        Task<EmployeeScheduleResponse> CreateSchedule(CreateEmployeeScheduleModel createScheduleDto);

    }
}
