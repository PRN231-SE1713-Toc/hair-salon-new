using AutoMapper;
using HairSalon.Core.Dtos.Requests;
using HairSalon.Core.Dtos.Responses;
using HairSalon.Core.Entities;

namespace HairSalon.Service
{
    public class MappingProfileExtension : Profile
    {
        public MappingProfileExtension()
        {
            CreateMap<Customer, LoginCustomerResponse>();

            CreateMap<Employee, LoginEmployeeResponse>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            CreateMap<Customer, CustomerResponse>();
            CreateMap<CreatedCustomerModel, Customer>();
            CreateMap<UpdatedCustomer, Customer>();

            CreateMap<Core.Entities.Service, HairServiceResponse>()
                .ForMember(dest => dest.EstimatedDuration, opt => opt.MapFrom(src => src.Duration))
                .ReverseMap();
            CreateMap<HairServiceRequest, Core.Entities.Service>();
            CreateMap<UpdateHairServiceRequest, Core.Entities.Service>();

            CreateMap<EmployeeSchedule, EmployeeScheduleResponse>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Employee.Name)); ;
            CreateMap<UpdateEmployeeSchedule, EmployeeSchedule>();
            CreateMap<CreateEmployeeScheduleModel, EmployeeSchedule>();

            CreateMap<AppointmentService, AppointmentServiceDto>().ReverseMap();
        }
    }
}
