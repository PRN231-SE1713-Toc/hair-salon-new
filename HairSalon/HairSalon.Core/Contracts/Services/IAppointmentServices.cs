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
    public interface IAppointmentServices
    {
        Task<List<AppointmentViewResponse>> GetAppointments();
        Task<List<AppointmentViewResponse>> GetAppointmentsbyCustomerId(int customerId, int status);
        Task<List<AppointmentViewResponse>> GetAppointmentsByStylistId(int stylistId, int status);
        Task<AppointmentViewResponse?> GetAppointment(int id);
        Task<string> CreateAppointment(AppointmentCreateModel newAppointment);
        Task<string> UpdateAppointment(AppointmentUpdateModel updatedAppointment);
        Task<string> DeleteAppointment(int id);
    }
}
