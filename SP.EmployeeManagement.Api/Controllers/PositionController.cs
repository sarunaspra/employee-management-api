using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using SP.EmployeeManagement.BusinessLogic.Services.IServices;
using SP.EmployeeManagement.BusinessLogic.Utilities.CustomExceptions;
using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.Api.Controllers
{
    /// <summary>
    /// Position Controller
    /// </summary>
    [ApiVersion("1.0")]
    [Route("v{version:apiVersion}/[controller]")]
    [ApiController]
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;
        private readonly IValidator<PositionDto> _positionDtoValidator;

        public PositionController(IPositionService positionService, IValidator<PositionDto> positionDtoValidator)
        {
            _positionService = positionService;
            _positionDtoValidator = positionDtoValidator;
        }

        /// <summary>
        /// Creates a new position
        /// </summary>
        /// <param name="position">Position request data</param>
        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePositionAsync(PositionDto position)
        {
            try
            {
                var validationResult = await _positionDtoValidator.ValidateAsync(position);

                if (validationResult.IsValid)
                {
                    await _positionService.CreatePositionAsync(position);

                    return CreatedAtAction(nameof(GetPositionById), new { id = position.Id }, position);
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
        /// Deletes a position
        /// </summary>
        /// <param name="positionId">Position id</param>
        [HttpDelete("delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePositionAsync(int positionId)
        {
            try
            {
                await _positionService.DeletePositionAsync(positionId);
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
        /// Returns one position by id
        /// </summary>
        /// <param name="id">Position id</param>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(PositionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPositionById(int id)
        {
            try
            {
                return Ok(await _positionService.GetPositionByIdAsync(id));
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
        /// Returns all positions
        /// </summary>
        [HttpGet("getAll")]
        [ProducesResponseType(typeof(List<PositionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPositionsAsync()
        {
            try
            {
                return Ok(await _positionService.GetPositionsAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred.");
            }
        }

        /// <summary>
        /// Updates data of already existing position
        /// </summary>
        /// <param name="position">Position request data</param>
        [HttpPut("update")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePositionAsync(PositionDto position)
        {
            try
            {
                var validationResult = await _positionDtoValidator.ValidateAsync(position);

                if (validationResult.IsValid)
                {
                    await _positionService.UpdatePositionAsync(position);

                    return Ok();
                }
                else
                {
                    return BadRequest(validationResult);
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
