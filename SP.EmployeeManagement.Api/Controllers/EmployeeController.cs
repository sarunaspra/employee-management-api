using Microsoft.AspNetCore.Mvc;
using SP.EmployeeManagement.BusinessLogic.Services.IServices;
using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeDto employee)
        {
            await _employeeService.CreateEmployee(employee);

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int employeeId)
        {
            await _employeeService.DeleteEmployee(employeeId);

            return Ok();
        }

        [HttpGet]
        [Route("/{id:int}")]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            return Ok(await _employeeService.GetEmployeeById(id));
        }

        [HttpGet]
        [Route("/GetAll")]
        public async Task<IActionResult> GetEmployeeList()
        {
            var users = await _employeeService.GetAllEmployees();
            return Ok(users);
        }

        [HttpPut]
        public async Task<IActionResult> Update(EmployeeDto employee)
        {
            await _employeeService.UpdateEmployee(employee);

            return Ok();
        }
    }
}
