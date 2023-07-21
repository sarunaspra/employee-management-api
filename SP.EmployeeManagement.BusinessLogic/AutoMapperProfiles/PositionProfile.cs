using AutoMapper;
using SP.EmployeeManagement.DataAccess.Entities;
using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.BusinessLogic.AutoMapperProfiles
{
    public class PositionProfile : Profile
    {
        public PositionProfile()
        {
            CreateMap<Position, PositionDto>()
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.Employees.Count));
            CreateMap<PositionDto, Position>()
                .ForPath(dest => dest.Employees.Count, opt => opt.MapFrom(src => src.EmployeeCount));
        }
    }
}
