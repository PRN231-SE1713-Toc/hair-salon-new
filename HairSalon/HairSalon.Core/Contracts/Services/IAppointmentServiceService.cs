using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Contracts.Services
{
    public interface IAppointmentServiceService
    {
        Task<IEnumerable<AppointmentService>> GetAllServicesAsync();
        Task<AppointmentService> GetServiceByIdAsync(int id);
        Task<AppointmentService> CreateServiceAsync(CreateAppointmentServiceModel model);
        Task<AppointmentService> UpdateServiceAsync(int id, UpdateAppointmentServiceModel model);
        Task DeleteServiceAsync(int id);
    }
}
