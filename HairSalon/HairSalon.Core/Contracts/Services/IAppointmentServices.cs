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
        Task<AppointmentViewResponse?> GetAppointment(int id);
        Task<bool> CreateAppointment(AppointmentCreateModel newAppointment);
        Task<bool> UpdateAppointment(Appointment updatedAppointment);
        Task<bool> DeleteAppointment(int id);
    }
}
