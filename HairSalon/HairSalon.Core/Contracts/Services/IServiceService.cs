using HairSalon.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HairSalon.Core.Contracts.Services
{
    public interface IServiceService
    {
        Task<List<Service>> GetServices();
        Task<Service> GetServicesById(int id);
        Task<Service> CreateService(Service service);
        Task<Service> UpdateService(int id, Service service);
    }
}
