using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.BusinessLogic.Services.IServices
{
    public interface IDepartmentService
    {
        Task CreateDepartmentAsync(DepartmentDto departmentDto);

        Task<List<DepartmentDto>> GetDepartmentsAsync();

        Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId);

        Task UpdateDepartmentAsync(DepartmentDto departmentToUpdateDto);

        Task DeleteDepartmentAsync(int departmentId);
    }
}
