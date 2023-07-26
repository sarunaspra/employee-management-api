using AutoMapper;
using SP.EmployeeManagement.DataAccess.Entities;
using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.BusinessLogic.Utilities.AutoMapperProfiles
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.EmployeeCount, opt => opt.MapFrom(src => src.Employees.Count));
            CreateMap<DepartmentDto, Department>()
                .ForPath(dest => dest.Employees.Count, opt => opt.MapFrom(src => src.EmployeeCount));
        }
    }
}
