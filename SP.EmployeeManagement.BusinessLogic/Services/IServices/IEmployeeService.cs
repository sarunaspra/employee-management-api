using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.BusinessLogic.Services.IServices
{
    public interface IEmployeeService
    {
        Task CreateEmployee(EmployeeDto employee);

        Task<List<EmployeeDto>> GetAllEmployees();

        Task<EmployeeDto> GetEmployeeById(int employeeId);

        Task UpdateEmployee(EmployeeDto employee);

        Task DeleteEmployee(int employeeId);
    }
}
