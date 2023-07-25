using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using SP.EmployeeManagement.BusinessLogic.Services;
using SP.EmployeeManagement.BusinessLogic.Utilities.CustomExceptions;
using SP.EmployeeManagement.BusinessLogic.Validators;
using SP.EmployeeManagement.DataAccess.Entities;
using SP.EmployeeManagement.DataAccess.Interfaces;
using SP.EmployeeManagement.Dto.Dtos;
using Xunit;

namespace SP.EmployeeManagement.Test.BusinessLogic.Services
{
    public class PositionServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IPositionRepository> _positionRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<PositionService>> _logger;
        private readonly PositionDtoValidator _positionDtoValidator;
        private readonly PositionService _positionService;

        public PositionServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _positionRepository = new Mock<IPositionRepository>();
            _unitOfWork.Setup(uow => uow.PositionRepository).Returns(_positionRepository.Object);

            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<PositionService>>();
            _positionDtoValidator = new PositionDtoValidator();

            _positionService = new PositionService(
                _unitOfWork.Object,
                _mapper.Object,
                _logger.Object,
                _positionDtoValidator
            );
        }

        [Fact]
        public async Task CreatePositionAsync_ValidPositionDto_PositionAdded()
        {
            //Arrange
            var positionDto = new PositionDto
            {
                Id = 1,
                Title = ".Net developer",
                Description = "Test",
                EmployeeCount = 0
            };

            //Act
            await _positionService.CreatePositionAsync(positionDto);

            //Assert
            _positionRepository.Verify(repo => repo.Add(It.IsAny<Position>()), Times.Once);

            _unitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

        [Fact]
        public async Task CreatePositionAsync_InvalidPositionDto_ThrowsInputValidationException()
        {
            // Arrange
            var positionDto = new PositionDto
            {
                Id = 1,
                Title = String.Empty,
                Description = "Test",
                EmployeeCount = 0
            };

            // Act and Assert
            await Assert.ThrowsAsync<InputValidationException>(() => _positionService.CreatePositionAsync(positionDto));
        }

        [Fact]
        public async Task DeletePositionAsync_ExistingPositionId_DeletesPosition()
        {
            //Arrange
            var position = new Position { Id = 5 };
            _unitOfWork.Setup(uow => uow.PositionRepository.GetById(position.Id)).ReturnsAsync(position);

            //Act
            await _positionService.DeletePositionAsync(position.Id);

            //Assert
            _positionRepository.Verify(repo => repo.Delete(It.IsAny<Position>()), Times.Once);

            _unitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

        [Fact]
        public async Task DeletePositionAsync_NonExistingPositionId_ThrowUserNotFoundException()
        {
            //Arrange
            var position = new Position { Id = 1 };

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _positionService.DeletePositionAsync(position.Id));
        }

        [Fact]
        public async Task GetPositionsAsync_WhenSuccess_ReturnsListOfPositionDto()
        {
            // Arrange
            var positionsList = new List<Position>
            {
                new Position { Id = 1, Title = ".Net developer", Description = ".Net developer description" },
                new Position { Id = 2, Title = "Java developer", Description = "Java developer description" }
            };

            _unitOfWork.Setup(uow => uow.PositionRepository.GetAll()).ReturnsAsync(positionsList);

            var expectedPositionDtos = positionsList.Select(e => new PositionDto { Id = e.Id, Title = e.Title, Description = e.Description }).ToList();

            _mapper.Setup(mapper => mapper.Map<List<PositionDto>>(positionsList)).Returns(expectedPositionDtos);

            // Act
            var result = await _positionService.GetPositionsAsync();

            // Assert
            _unitOfWork.Verify(repo => repo.PositionRepository.GetAll(), Times.Once);
            _mapper.Verify(mapper => mapper.Map<List<PositionDto>>(positionsList), Times.Once);
            Assert.Equal(expectedPositionDtos, result);
        }

        [Fact]
        public async Task GetPositionByIdAsync_ExistingPositionId_ReturnsPositionDto()
        {
            // Arrange
            int positionId = 1;
            var positionToReturn = new Position { Id = positionId, Title = ".Net developer", Description = ".Net developer description" };

            _unitOfWork.Setup(uow => uow.PositionRepository.GetById(positionId)).ReturnsAsync(positionToReturn);

            var expectedPositionDto = new PositionDto { Id = positionId, Title = ".Net developer", Description = ".Net developer description" };

            _mapper.Setup(mapper => mapper.Map<PositionDto>(positionToReturn)).Returns(expectedPositionDto);

            // Act
            var result = await _positionService.GetPositionByIdAsync(positionId);

            // Assert
            _unitOfWork.Verify(repo => repo.PositionRepository.GetById(positionId), Times.Once);
            _mapper.Verify(mapper => mapper.Map<PositionDto>(positionToReturn), Times.Once);
            Assert.Equal(expectedPositionDto, result);
        }

        [Fact]
        public async Task GetPositionByIdAsync_NonExistingPositionId_ThrowsUserNotFoundException()
        {
            // Arrange
            int positionId = 999;
            _unitOfWork.Setup(uow => uow.PositionRepository.GetById(positionId)).ReturnsAsync((Position)null);

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _positionService.GetPositionByIdAsync(positionId));
        }

        [Fact]
        public async Task UpdatePositionAsync_ValidPositionDto_PositionUpdated()
        {
            // Arrange
            var positionToUpdateDto = new PositionDto
            {
                Id = 1,
                Title = ".Net developer",
                Description = ".Net developer description"
            };

            var existingPosition = new Position
            {
                Id = 1,
                Title = "Existing name",
                Description = "Existing description"
            };

            _unitOfWork.Setup(uow => uow.PositionRepository.GetById(positionToUpdateDto.Id)).ReturnsAsync(existingPosition);


            // Act
            await _positionService.UpdatePositionAsync(positionToUpdateDto);

            // Assert
            Assert.Equal(positionToUpdateDto.Title, existingPosition.Title);
            Assert.Equal(positionToUpdateDto.Description, existingPosition.Description);

            _unitOfWork.Verify(repo => repo.PositionRepository.Update(existingPosition), Times.Once);
            _unitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

        [Fact]
        public async Task UpdatePositionAsync_NonExistingPosition_ThrowsUserNotFoundException()
        {
            // Arrange
            var positionToUpdateDto = new PositionDto
            {
                Id = 999,
                Title = "Java developer",
                Description = "Example",
            };

            _unitOfWork.Setup(uow => uow.PositionRepository.GetById(positionToUpdateDto.Id)).ReturnsAsync((Position)null);

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _positionService.UpdatePositionAsync(positionToUpdateDto));
        }

        [Fact]
        public async Task UpdatePositionAsync_InvalidPositionDto_ThrowsInputValidationException()
        {
            // Arrange
            var positionToUpdateDto = new PositionDto
            {
                Id = 1,
                Title = String.Empty,
                Description = "Example"
            };

            var existingPosition = new Position
            {
                Id = 1,
                Title = "Existing name",
                Description = "Existing description"
            };

            _unitOfWork.Setup(uow => uow.PositionRepository.GetById(positionToUpdateDto.Id)).ReturnsAsync(existingPosition);

            // Act and Assert
            await Assert.ThrowsAsync<InputValidationException>(() => _positionService.UpdatePositionAsync(positionToUpdateDto));
        }
    }
}
