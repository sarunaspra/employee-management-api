using AutoMapper;
using SP.EmployeeManagement.DataAccess.Entities;
using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.BusinessLogic.AutoMapperProfiles
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}
