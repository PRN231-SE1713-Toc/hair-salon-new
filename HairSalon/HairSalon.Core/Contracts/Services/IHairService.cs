using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;

namespace HairSalon.Core.Contracts.Services
{
    public interface IHairService
    {
        Task<HairServiceResponse> GetService(int id);
        
        Task<Service?> GetServiceById(int id);
        
        Task<bool> CreateService(HairServiceRequest serviceRequest);
        
        Task<bool> UpdateService(UpdateHairServiceRequest request);

        Task<bool> DeleteService(Service service);
        
        Task<IList<HairServiceResponse>> GetHairServices();
    }
}
