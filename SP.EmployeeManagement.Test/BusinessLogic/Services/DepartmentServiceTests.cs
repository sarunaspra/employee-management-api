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
    public class DepartmentServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IDepartmentRepository> _departmentRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<DepartmentService>> _logger;
        private readonly DepartmentDtoValidator _departmentDtoValidator;
        private readonly DepartmentService _departmentService;

        public DepartmentServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _departmentRepository = new Mock<IDepartmentRepository>();
            _unitOfWork.Setup(uow => uow.DepartmentRepository).Returns(_departmentRepository.Object);

            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<DepartmentService>>();
            _departmentDtoValidator = new DepartmentDtoValidator();

            _departmentService = new DepartmentService(
                _unitOfWork.Object,
                _mapper.Object,
                _logger.Object,
                _departmentDtoValidator
            );
        }

        [Fact]
        public async Task CreateDepartmentAsync_ValidDepartmentDto_DepartmentAdded()
        {
            //Arrange
            var departmentDto = new DepartmentDto
            {
                Id = 1,
                Name = ".Net",
                Description = "Test",
                EmployeeCount = 0
            };

            //Act
            await _departmentService.CreateDepartmentAsync(departmentDto);

            //Assert
            _departmentRepository.Verify(repo => repo.Add(It.IsAny<Department>()), Times.Once);

            _unitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

        [Fact]
        public async Task CreateDepartmentAsync_InvalidDepartmentDto_ThrowsInputValidationException()
        {
            // Arrange
            var departmentDto = new DepartmentDto
            {
                Id = 1,
                Name = String.Empty,
                Description = "Test",
                EmployeeCount = 0
            };

            // Act and Assert
            await Assert.ThrowsAsync<InputValidationException>(() => _departmentService.CreateDepartmentAsync(departmentDto));
        }

        [Fact]
        public async Task DeleteDepartmentAsync_ExistingDepartmentId_DeletesDepartment()
        {
            //Arrange
            var department = new Department { Id = 5 };
            _unitOfWork.Setup(uow => uow.DepartmentRepository.GetById(department.Id)).ReturnsAsync(department);

            //Act
            await _departmentService.DeleteDepartmentAsync(department.Id);

            //Assert
            _departmentRepository.Verify(repo => repo.Delete(It.IsAny<Department>()), Times.Once);

            _unitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

        [Fact]
        public async Task DeleteDepartmentAsync_NonExistingDepartmentId_ThrowUserNotFoundException()
        {
            //Arrange
            var department = new Department { Id = 1 };

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _departmentService.DeleteDepartmentAsync(department.Id));
        }

        [Fact]
        public async Task GetDepartmentsAsync_WhenSuccess_ReturnsListOfDepartmentDto()
        {
            // Arrange
            var departmentsList = new List<Department>
            {
                new Department { Id = 1, Name = ".Net", Description = ".Net description" },
                new Department { Id = 2, Name = "Java", Description = "Java description" }
            };

            _unitOfWork.Setup(uow => uow.DepartmentRepository.GetAll()).ReturnsAsync(departmentsList);

            var expectedDepartmentDtos = departmentsList.Select(e => new DepartmentDto { Id = e.Id, Name = e.Name, Description = e.Description }).ToList();

            _mapper.Setup(mapper => mapper.Map<List<DepartmentDto>>(departmentsList)).Returns(expectedDepartmentDtos);

            // Act
            var result = await _departmentService.GetDepartmentsAsync();

            // Assert
            _unitOfWork.Verify(repo => repo.DepartmentRepository.GetAll(), Times.Once);
            _mapper.Verify(mapper => mapper.Map<List<DepartmentDto>>(departmentsList), Times.Once);
            Assert.Equal(expectedDepartmentDtos, result);
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_ExistingDepartmentId_ReturnsDepartmentDto()
        {
            // Arrange
            int departmentId = 1;
            var departmentToReturn = new Department { Id = departmentId, Name = ".Net", Description = ".Net description" };

            _unitOfWork.Setup(uow => uow.DepartmentRepository.GetById(departmentId)).ReturnsAsync(departmentToReturn);

            var expectedDepartmentDto = new DepartmentDto { Id = departmentId, Name = ".Net", Description = ".Net description" };

            _mapper.Setup(mapper => mapper.Map<DepartmentDto>(departmentToReturn)).Returns(expectedDepartmentDto);

            // Act
            var result = await _departmentService.GetDepartmentByIdAsync(departmentId);

            // Assert
            _unitOfWork.Verify(repo => repo.DepartmentRepository.GetById(departmentId), Times.Once);
            _mapper.Verify(mapper => mapper.Map<DepartmentDto>(departmentToReturn), Times.Once);
            Assert.Equal(expectedDepartmentDto, result);
        }

        [Fact]
        public async Task GetDepartmentByIdAsync_NonExistingDepartmentId_ThrowsUserNotFoundException()
        {
            // Arrange
            int departmentId = 999;
            _unitOfWork.Setup(uow => uow.DepartmentRepository.GetById(departmentId)).ReturnsAsync((Department)null);

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _departmentService.GetDepartmentByIdAsync(departmentId));
        }

        [Fact]
        public async Task UpdateDepartmentAsync_ValidDepartmentDto_DepartmentUpdated()
        {
            // Arrange
            var departmentToUpdateDto = new DepartmentDto
            {
                Id = 1,
                Name = ".Net",
                Description = ".Net description"
            };

            var existingDepartment = new Department
            {
                Id = 1,
                Name = "Existing name",
                Description = "Existing description"
            };

            _unitOfWork.Setup(uow => uow.DepartmentRepository.GetById(departmentToUpdateDto.Id)).ReturnsAsync(existingDepartment);


            // Act
            await _departmentService.UpdateDepartmentAsync(departmentToUpdateDto);

            // Assert
            Assert.Equal(departmentToUpdateDto.Name, existingDepartment.Name);
            Assert.Equal(departmentToUpdateDto.Description, existingDepartment.Description);

            _unitOfWork.Verify(repo => repo.DepartmentRepository.Update(existingDepartment), Times.Once);
            _unitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

        [Fact]
        public async Task UpdateDepartmentAsync_NonExistingDepartment_ThrowsUserNotFoundException()
        {
            // Arrange
            var departmentToUpdateDto = new DepartmentDto
            {
                Id = 999,
                Name = "Java",
                Description = "Example",
            };

            _unitOfWork.Setup(uow => uow.DepartmentRepository.GetById(departmentToUpdateDto.Id)).ReturnsAsync((Department)null);

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _departmentService.UpdateDepartmentAsync(departmentToUpdateDto));
        }

        [Fact]
        public async Task UpdateDepartmentAsync_InvalidDepartmentDto_ThrowsInputValidationException()
        {
            // Arrange
            var departmentToUpdateDto = new DepartmentDto
            {
                Id = 1,
                Name = String.Empty,
                Description = "Example"
            };

            var existingDepartment = new Department
            {
                Id = 1,
                Name = "Existing name",
                Description = "Existing description"
            };

            _unitOfWork.Setup(uow => uow.DepartmentRepository.GetById(departmentToUpdateDto.Id)).ReturnsAsync(existingDepartment);

            // Act and Assert
            await Assert.ThrowsAsync<InputValidationException>(() => _departmentService.UpdateDepartmentAsync(departmentToUpdateDto));
        }
    }
}
