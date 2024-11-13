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
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Employee.Name));
            CreateMap<EmployeeScheduleResponse, EmployeeSchedule>();

            CreateMap<UpdateEmployeeSchedule, EmployeeSchedule>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.WorkingStartTime, opt => opt.MapFrom(src =>
                    TryParseTime(src.WorkingStartTime)))
                .ForMember(dest => dest.WorkingEndTime, opt => opt.MapFrom(src =>
                    TryParseTime(src.WorkingEndTime)));

            CreateMap<CreateEmployeeScheduleModel, EmployeeSchedule>()
                .ForMember(dest => dest.WorkingStartTime, opt => opt.MapFrom(src => TimeOnly.Parse(src.WorkingStartTime)))
                .ForMember(dest => dest.WorkingEndTime, opt => opt.MapFrom(src => TimeOnly.Parse(src.WorkingEndTime)));

            CreateMap<AppointmentService, AppointmentServiceDto>().ReverseMap();
        }

        private TimeOnly TryParseTime(string timeString)
        {
            if (TimeOnly.TryParse(timeString, out var parsedTime))
            {
                return parsedTime;
            }
            else
            {
                return TimeOnly.MinValue;
            }
        }

    }

}
