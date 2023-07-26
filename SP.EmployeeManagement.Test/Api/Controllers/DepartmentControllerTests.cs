using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SP.EmployeeManagement.Api.Controllers;
using SP.EmployeeManagement.BusinessLogic.Services.IServices;
using SP.EmployeeManagement.Dto.Dtos;
using Xunit;

namespace SP.EmployeeManagement.Test.Api.Controllers
{
    public class DepartmentControllerTests
    {
        private readonly Mock<IDepartmentService> _departmentService;
        private readonly DepartmentController _departmentController;

        public DepartmentControllerTests()
        {
            _departmentService = new Mock<IDepartmentService>();
            _departmentController = new DepartmentController(_departmentService.Object);
        }

        [Fact]
        public async Task CreateDepartmentAsync_ValidDepartmentData_ReturnsCreatedResult()
        {
            // Arrange
            var departmentDto = new DepartmentDto
            {
                Id = 1,
                Name = ".Net department",
                Description = ".Net department description",
                EmployeeCount = 0
            };

            _departmentService.Setup(x => x.CreateDepartmentAsync(It.IsAny<DepartmentDto>())).Verifiable();

            // Act
            var result = await _departmentController.CreateDepartmentAsync(departmentDto);

            // Assert
            var createdResult = (CreatedAtActionResult)result;
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(departmentDto, createdResult.Value);
        }

        [Fact]
        public async Task DeleteDepartmentAsync_ExistingDepartment_DeletesDepartmentAndReturnsNoContent()
        {
            // Arrange
            int departmentIdToDelete = 1;
            _departmentService
                .Setup(x => x.DeleteDepartmentAsync(It.IsAny<int>()))
                .Verifiable();

            // Act
            var result = await _departmentController.DeleteDepartmentAsync(departmentIdToDelete);

            // Assert
            var noContentResult = (NoContentResult)result;
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
            _departmentService.Verify(x => x.DeleteDepartmentAsync(departmentIdToDelete), Times.Once);
        }

        [Fact]
        public async Task GetDepartmentById_ExistingDepartment_ReturnsOkWithDepartmentDto()
        {
            // Arrange
            int existingDepartmentId = 1;
            var departmentDto = new DepartmentDto
            {
                Id = 1,
                Name = ".Net department",
                Description = ".Net department description",
                EmployeeCount = 0
            };

            _departmentService
                .Setup(x => x.GetDepartmentByIdAsync(existingDepartmentId))
                .ReturnsAsync(departmentDto);

            // Act
            var result = await _departmentController.GetDepartmentById(existingDepartmentId);

            // Assert
            var okResult = (OkObjectResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedDepartmentDto = (DepartmentDto)okResult.Value;
            Assert.Equal(existingDepartmentId, returnedDepartmentDto.Id);
        }

        [Fact]
        public async Task GetDepartmentsAsync_ReturnsOkWithListOfDepartmentDto()
        {
            // Arrange
            var departments = new List<DepartmentDto>
        {
            new DepartmentDto { Id = 1, Name = ",Net department", Description = ".Net department description" },
            new DepartmentDto { Id = 2, Name = "Java department", Description = "Java department description" }
        };

            _departmentService
                .Setup(x => x.GetDepartmentsAsync())
                .ReturnsAsync(departments);

            // Act
            var result = await _departmentController.GetDepartmentsAsync();

            // Assert
            var okResult = (OkObjectResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedDepartments = (List<DepartmentDto>)okResult.Value;
            Assert.Equal(departments, returnedDepartments);
        }

        [Fact]
        public async Task UpdateDepartmentAsync_ExistingDepartment_ReturnsOk()
        {
            // Arrange
            var departmentDto = new DepartmentDto
            {
                Id = 1,
                Name = "Java department",
                Description = "Java department description"
            };

            _departmentService
                .Setup(x => x.UpdateDepartmentAsync(It.IsAny<DepartmentDto>()))
                .Verifiable();

            // Act
            var result = await _departmentController.UpdateDepartmentAsync(departmentDto);

            // Assert
            var okResult = (OkResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _departmentService.Verify(x => x.UpdateDepartmentAsync(departmentDto), Times.Once);
        }
    }
}
