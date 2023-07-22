using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SP.EmployeeManagement.BusinessLogic.Services.IServices;
using SP.EmployeeManagement.BusinessLogic.Utilities.CustomExceptions;
using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.Api.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IValidator<EmployeeDto> _employeeDtoValidator;

        public EmployeeController(IEmployeeService employeeService, IValidator<EmployeeDto> employeeDtoValidator)
        {
            _employeeService = employeeService;
            _employeeDtoValidator = employeeDtoValidator;
        }

        /// <summary>
        /// Creates a new employee
        /// </summary>
        /// <param name="employee">Employee request data</param>
        [HttpPost("[controller]/Create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateEmployeeAsync(EmployeeDto employee)
        {
            try
            {
                var validationResult = await _employeeDtoValidator.ValidateAsync(employee);

                if (validationResult.IsValid) 
                { 
                    await _employeeService.CreateEmployeeAsync(employee);
                
                    return CreatedAtAction(nameof(GetEmployeeById), new {id = employee.Id}, employee);
                }
                else
                {
                    return BadRequest(validationResult.Errors);
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Deletes an employee
        /// </summary>
        /// <param name="employeeId">Employee id</param>
        [HttpDelete("[controller]/Delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                await _employeeService.DeleteEmployeeAsync(employeeId);
                return NoContent();
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Returns one employee by id
        /// </summary>
        /// <param name="id">Employee id</param>
        [HttpGet("[controller]/{id:int}")]
        [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeeById(int id)
        {
            try
            {
                return Ok(await _employeeService.GetEmployeeByIdAsync(id));
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Returns all employees
        /// </summary>
        [HttpGet("[controller]/GetAll")]
        [ProducesResponseType(typeof(List<EmployeeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetEmployeesAsync()
        {
            try
            {
                return Ok(await _employeeService.GetEmployeesAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Updates data of already existing employee
        /// </summary>
        /// <param name="employee">Employee request data</param>
        [HttpPut("[controller]/Update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateEmployeeAsync(EmployeeDto employee)
        {
            try
            {
                var validationResult = await _employeeDtoValidator.ValidateAsync(employee);

                if (validationResult.IsValid)
                {
                    await _employeeService.UpdateEmployeeAsync(employee);

                    return Ok();
                }
                else
                {
                    return BadRequest(validationResult.Errors);
                }
            }
            catch (UserNotFoundException)
            {
                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }
    }
}
