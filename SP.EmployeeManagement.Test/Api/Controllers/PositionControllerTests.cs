using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SP.EmployeeManagement.Api.Controllers;
using SP.EmployeeManagement.BusinessLogic.Services.IServices;
using SP.EmployeeManagement.Dto.Dtos;
using Xunit;

namespace SP.EmployeeManagement.Test.Api.Controllers
{
    public class PositionControllerTests
    {
        private readonly Mock<IPositionService> _positionService;
        private readonly PositionController _positionController;

        public PositionControllerTests()
        {
            _positionService = new Mock<IPositionService>();
            _positionController = new PositionController(_positionService.Object);
        }

        [Fact]
        public async Task CreatePositionAsync_ValidPositionData_ReturnsCreatedResult()
        {
            // Arrange
            var positionDto = new PositionDto
            {
                Id = 1,
                Title = ".Net developer",
                Description = ".Net developer description",
                EmployeeCount = 0
            };

            _positionService.Setup(x => x.CreatePositionAsync(It.IsAny<PositionDto>())).Verifiable();

            // Act
            var result = await _positionController.CreatePositionAsync(positionDto);

            // Assert
            var createdResult = (CreatedAtActionResult)result;
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.Equal(positionDto, createdResult.Value);
        }

        [Fact]
        public async Task DeletePositionAsync_ExistingPosition_DeletesPositionAndReturnsNoContent()
        {
            // Arrange
            int positionIdToDelete = 1;
            _positionService
                .Setup(x => x.DeletePositionAsync(It.IsAny<int>()))
                .Verifiable();

            // Act
            var result = await _positionController.DeletePositionAsync(positionIdToDelete);

            // Assert
            var noContentResult = (NoContentResult)result;
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
            _positionService.Verify(x => x.DeletePositionAsync(positionIdToDelete), Times.Once);
        }

        [Fact]
        public async Task GetPositionById_ExistingPosition_ReturnsOkWithPositionDto()
        {
            // Arrange
            int existingPositionId = 1;
            var positionDto = new PositionDto
            {
                Id = 1,
                Title = ".Net developer",
                Description = ".Net developer description",
                EmployeeCount = 0
            };

            _positionService
                .Setup(x => x.GetPositionByIdAsync(existingPositionId))
                .ReturnsAsync(positionDto);

            // Act
            var result = await _positionController.GetPositionById(existingPositionId);

            // Assert
            var okResult = (OkObjectResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedPositionDto = (PositionDto)okResult.Value;
            Assert.Equal(existingPositionId, returnedPositionDto.Id);
        }

        [Fact]
        public async Task GetPositionsAsync_ReturnsOkWithListOfPositionDto()
        {
            // Arrange
            var positions = new List<PositionDto>
        {
            new PositionDto { Id = 1, Title = ",Net developer", Description = ".Net developer description" },
            new PositionDto { Id = 2, Title = "Java developer", Description = "Java developer description" }
        };

            _positionService
                .Setup(x => x.GetPositionsAsync())
                .ReturnsAsync(positions);

            // Act
            var result = await _positionController.GetPositionsAsync();

            // Assert
            var okResult = (OkObjectResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            var returnedPositions = (List<PositionDto>)okResult.Value;
            Assert.Equal(positions, returnedPositions);
        }

        [Fact]
        public async Task UpdatePositionAsync_ExistingPosition_ReturnsOk()
        {
            // Arrange
            var positionDto = new PositionDto
            {
                Id = 1,
                Title = "Java developer",
                Description = "Java developer description"
            };

            _positionService
                .Setup(x => x.UpdatePositionAsync(It.IsAny<PositionDto>()))
                .Verifiable();

            // Act
            var result = await _positionController.UpdatePositionAsync(positionDto);

            // Assert
            var okResult = (OkResult)result;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            _positionService.Verify(x => x.UpdatePositionAsync(positionDto), Times.Once);
        }
    }
}
