using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SP.EmployeeManagement.BusinessLogic.Services.IServices;
using SP.EmployeeManagement.BusinessLogic.Utilities.CustomExceptions;
using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.Api.Controllers
{
    /// <summary>
    /// Department Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentService _departmentService;
        private readonly IValidator<DepartmentDto> _departmentDtoValidator;

        public DepartmentController(IDepartmentService departmentService, IValidator<DepartmentDto> departmentDtoValidator)
        {
            _departmentService = departmentService;
            _departmentDtoValidator = departmentDtoValidator;
        }

        /// <summary>
        /// Creates a new department
        /// </summary>
        /// <param name="department">Department request data</param>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateDepartmentAsync(DepartmentDto department)
        {
            try
            {
                var validationResult = await _departmentDtoValidator.ValidateAsync(department);

                if (validationResult.IsValid)
                {
                    await _departmentService.CreateDepartmentAsync(department);

                    return CreatedAtAction(nameof(GetDepartmentById), new { id = department.Id }, department);
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
        /// Deletes a department
        /// </summary>
        /// <param name="departmentId">Department id</param>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteDepartmentAsync(int departmentId)
        {
            try
            {
                await _departmentService.DeleteDepartmentAsync(departmentId);
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
        /// Returns one department by id
        /// </summary>
        /// <param name="id">Department id</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(DepartmentDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDepartmentById(int id)
        {
            try
            {
                return Ok(await _departmentService.GetDepartmentByIdAsync(id));
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
        /// Returns all departments
        /// </summary>
        [HttpGet("getAll")]
        [ProducesResponseType(typeof(List<DepartmentDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDepartmentsAsync()
        {
            try
            {
                return Ok(await _departmentService.GetDepartmentsAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Updates data of already existing department
        /// </summary>
        /// <param name="department">Department request data</param>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateDepartmentAsync(DepartmentDto department)
        {
            try
            {
                var validationResult = await _departmentDtoValidator.ValidateAsync(department);

                if (validationResult.IsValid)
                {
                    await _departmentService.UpdateDepartmentAsync(department);

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
