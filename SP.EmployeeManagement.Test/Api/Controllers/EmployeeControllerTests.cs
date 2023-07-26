using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SP.EmployeeManagement.Api.Controllers;
using SP.EmployeeManagement.BusinessLogic.Services.IServices;
using SP.EmployeeManagement.Dto.Dtos;
using Xunit;

namespace SP.EmployeeManagement.Test.Api.Controllers
{
    public class EmployeeControllerTests
    {
        private readonly Mock<IEmployeeService> _employeeService;
        private readonly EmployeeController _employeeController;

        public EmployeeControllerTests()
        {
            _employeeService = new Mock<IEmployeeService>();
            _employeeController = new EmployeeController(_employeeService.Object);
        }

        [Fact]
        public async Task CreateEmployeeAsync_ValidEmployeeData_ReturnsCreatedResult()
        {
            // Arrange
            var employeeDto = new EmployeeDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "John@gmail.com",
                PhoneNumber = "+37063555174",
                DepartmentId = 1,
                PositionId = 1,
                Salary = 4000
            };

            _employeeService.Setup(x => x.CreateEmployeeAsync(It.IsAny<EmployeeDto>())).Verifiable();

            // Act
            var result = await _employeeController.CreateEmployeeAsync(employeeDto);

            // Assert
            var createdResult = (CreatedAtActionResult)result;
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(employeeDto, createdResult.Value);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ExistingEmployee_DeletesEmployeeAndReturnsNoContent()
        {
            // Arrange
            int employeeIdToDelete = 1;
            _employeeService
                .Setup(x => x.DeleteEmployeeAsync(It.IsAny<int>()))
                .Verifiable();

            // Act
            var result = await _employeeController.DeleteEmployeeAsync(employeeIdToDelete);

            // Assert
            var noContentResult = (NoContentResult)result;
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
            _employeeService.Verify(x => x.DeleteEmployeeAsync(employeeIdToDelete), Times.Once);
        }

        [Fact]
        public async Task GetEmployeeById_ExistingEmployee_ReturnsOkWithEmployeeDto()
        {
            // Arrange
            int existingEmployeeId = 1;
            var employeeDto = new EmployeeDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "John@gmail.com",
                PhoneNumber = "+37063555174",
                DepartmentId = 1,
                PositionId = 1,
                Salary = 4000
            };

            _employeeService
                .Setup(x => x.GetEmployeeByIdAsync(existingEmployeeId))
                .ReturnsAsync(employeeDto);

            // Act
            var result = await _employeeController.GetEmployeeById(existingEmployeeId);

            // Assert
            var okResult = (OkObjectResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedEmployeeDto = (EmployeeDto)okResult.Value;
            Assert.Equal(existingEmployeeId, returnedEmployeeDto.Id);
        }

        [Fact]
        public async Task GetEmployeesAsync_ReturnsOkWithListOfEmployeeDto()
        {
            // Arrange
            var employees = new List<EmployeeDto>
        {
            new EmployeeDto { Id = 1, FirstName = "John", LastName = "Doe" },
            new EmployeeDto { Id = 2, FirstName = "Jane", LastName = "Smith" },
        };

            _employeeService
                .Setup(x => x.GetEmployeesAsync())
                .ReturnsAsync(employees);

            // Act
            var result = await _employeeController.GetEmployeesAsync();

            // Assert
            var okResult = (OkObjectResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedEmployees = (List<EmployeeDto>)okResult.Value;
            Assert.Equal(employees, returnedEmployees);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ExistingEmployee_ReturnsOk()
        {
            // Arrange
            var employeeDto = new EmployeeDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                // Add other properties as needed
            };

            _employeeService
                .Setup(x => x.UpdateEmployeeAsync(It.IsAny<EmployeeDto>()))
                .Verifiable();

            // Act
            var result = await _employeeController.UpdateEmployeeAsync(employeeDto);

            // Assert
            var okResult = (OkResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _employeeService.Verify(x => x.UpdateEmployeeAsync(employeeDto), Times.Once);
        }
    }
}
