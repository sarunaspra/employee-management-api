using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.BusinessLogic.Services.IServices
{
    public interface IEmployeeService
    {
        Task CreateEmployeeAsync(EmployeeDto employeeDto);

        Task<List<EmployeeDto>> GetAllEmployeesAsync();

        Task<EmployeeDto> GetEmployeeByIdAsync(int employeeId);

        Task UpdateEmployeeAsync(EmployeeDto employeeToUpdateDto);

        Task DeleteEmployeeAsync(int employeeId);
    }
}
