using AutoMapper;
using HairSalon.Core;
using HairSalon.Core.Commons;
using HairSalon.Core.Contracts.Services;
using HairSalon.Core.Dtos.PaginationDtos;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace HairSalon.Service
{
    public class HairService : IHairService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HairService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        
        public async Task<bool> CreateService(HairServiceRequest serviceRequest)
        {
            try
            {
                if (serviceRequest is null) return false;
                // check duplicate of service
                var existedService = await _unitOfWork.ServiceRepository
                    .GetAsync(s => s.Name == serviceRequest.Name && s.Price == serviceRequest.Price);
                if (existedService is not null) return false;
                var service = _mapper.Map<Core.Entities.Service>(serviceRequest);
                
                _unitOfWork.ServiceRepository.Add(service);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<bool> DeleteService(Core.Entities.Service service)
        {
            try
            {
                if (service is null) return false;
                // Check if service is used in any appointment
                var data = await _unitOfWork.AppointmentServiceRepository.GetAll()
                    .AsNoTracking()
                    .Where(ap => ap.ServiceId == service.Id)
                    .ToListAsync();
                if (data.Any()) return false;
                
                _unitOfWork.ServiceRepository.Delete(service);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }

        public async Task<IList<HairServiceResponse>> GetHairServices()
        {
            var services = await _unitOfWork.ServiceRepository.GetAll()
                .AsNoTracking()
                .OrderBy(hs => hs.Id)
                .ToListAsync();
            return _mapper.Map<IList<HairServiceResponse>>(services);
        }

        public async Task<HairServiceResponse> GetService(int id)
            => _mapper.Map<HairServiceResponse>(await _unitOfWork.ServiceRepository
                .GetAsync(hs => hs.Id == id));

        public async Task<Core.Entities.Service?> GetServiceById(int id)
            => await _unitOfWork.ServiceRepository.FindByIdAsync(id);

        public async Task<bool> UpdateService(UpdateHairServiceRequest request)
        {
            try
            {
                var service = await _unitOfWork.ServiceRepository.FindByIdAsync(request.Id);
                if (service is null) return false;
                _ = _mapper.Map(request, service);
                _unitOfWork.ServiceRepository.Update(service);
                await _unitOfWork.CommitAsync();
                return true;
            }
            catch
            {
                await _unitOfWork.RollbackAsync();
                return false;
            }
        }
        public async Task<Pagination<Core.Entities.Service>> GetHairServiceByFilterAsync(PaginationParameter paginationParameter, ServiceFilterDTO serviceFilterDTO)
        {
            try
            {
                var services = await _unitOfWork.ServiceRepository.GetServiceByFilterAsync(paginationParameter, serviceFilterDTO);
                if (services != null)
                {
                    var mapperResult = _mapper.Map<List<Core.Entities.Service>>(services);
                    return new Pagination<Core.Entities.Service>(mapperResult, services.TotalCount, services.CurrentPage, services.PageSize);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*private void ValidateService(Core.Entities.Service service)
        {
            if (string.IsNullOrWhiteSpace(service.Name) || service.Name.Length > 100)
                throw new ArgumentException("Service Name is required and cannot exceed 100 characters.");

            if (!string.IsNullOrEmpty(service.Description) && service.Description.Length > 500)
                throw new ArgumentException("Description cannot exceed 500 characters.");

            if (!string.IsNullOrEmpty(service.Duration))
            {
                var regex = new System.Text.RegularExpressions.Regex(@"^\d{1,3}:\d{2}$");
                if (!regex.IsMatch(service.Duration))
                    throw new ArgumentException("Duration must be in the format of HH:mm.");
            }

            if (service.Price < 0)
                throw new ArgumentException("Price must be greater than 0.");
        }*/
    }
}
