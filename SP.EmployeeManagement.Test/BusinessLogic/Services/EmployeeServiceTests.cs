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
    public class EmployeeServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<IEmployeeRepository> _employeeRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILogger<EmployeeService>> _logger;
        private readonly EmployeeDtoValidator _employeeDtoValidator;
        private readonly EmployeeService _employeeService;

        public EmployeeServiceTests()
        {
            _unitOfWork = new Mock<IUnitOfWork>();
            _employeeRepository = new Mock<IEmployeeRepository>();
            _unitOfWork.Setup(uow => uow.EmployeeRepository).Returns(_employeeRepository.Object);

            _mapper = new Mock<IMapper>();
            _logger = new Mock<ILogger<EmployeeService>>();
            _employeeDtoValidator = new EmployeeDtoValidator();

            _employeeService = new EmployeeService(
                _unitOfWork.Object,
                _mapper.Object,
                _logger.Object,
                _employeeDtoValidator
            );
        }

        [Fact]
        public async Task CreateEmployeeAsync_ValidEmployeeDto_EmployeeAdded()
        {
            //Arrange
            var employeeDto = new EmployeeDto 
            { 
                Id = 1, 
                FirstName = "Name",
                LastName = "LastName",
                Email = "email@email.com",
                PhoneNumber = "+37063444111",
                DepartmentId = 1,
                PositionId = 1,
                Salary = 5000 
            };

            //Act
            await _employeeService.CreateEmployeeAsync(employeeDto);

            //Assert
            _employeeRepository.Verify(repo => repo.Add(It.IsAny<Employee>()), Times.Once);

            _unitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

        [Fact]
        public async Task CreateEmployeeAsync_InvalidEmployeeDto_ThrowsInputValidationException()
        {
            // Arrange
            var employeeDto = new EmployeeDto 
            {
                Id = 1,
                FirstName = "Name",
                LastName = "LastName",
                Email = "InvalidEmail",
                PhoneNumber = "8888888",
                DepartmentId = 1,
                PositionId = 1,
                Salary = 2000
            };

            // Act and Assert
            await Assert.ThrowsAsync<InputValidationException>(() => _employeeService.CreateEmployeeAsync(employeeDto));
        }

        [Fact]
        public async Task DeleteEmployeeAsync_ExistingEmployeeId_DeletesEmployee()
        {
            //Arrange
            var employee = new Employee { Id = 5 };
            _unitOfWork.Setup(uow => uow.EmployeeRepository.GetById(employee.Id)).ReturnsAsync(employee);

            //Act
            await _employeeService.DeleteEmployeeAsync(employee.Id);

            //Assert
            _employeeRepository.Verify(repo => repo.Delete(It.IsAny<Employee>()), Times.Once);

            _unitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_NonExistingEmployeeId_ThrowUserNotFoundException()
        {
            //Arrange
            var employee = new Employee { Id = 1 };

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _employeeService.DeleteEmployeeAsync(employee.Id));
        }

        [Fact]
        public async Task GetEmployeesAsync_WhenSuccess_ReturnsListOfEmployeeDto()
        {
            // Arrange
            var employeesList = new List<Employee>
            {
                new Employee { Id = 1, FirstName = "John", LastName = "Doe" },
                new Employee { Id = 2, FirstName = "Jane", LastName = "Smith" }
            };

            _unitOfWork.Setup(uow => uow.EmployeeRepository.GetAll()).ReturnsAsync(employeesList);

            var expectedEmployeeDtos = employeesList.Select(e => new EmployeeDto { Id = e.Id, FirstName = e.FirstName, LastName = e.LastName }).ToList();

            _mapper.Setup(mapper => mapper.Map<List<EmployeeDto>>(employeesList)).Returns(expectedEmployeeDtos);

            // Act
            var result = await _employeeService.GetEmployeesAsync();

            // Assert
            _unitOfWork.Verify(repo => repo.EmployeeRepository.GetAll(), Times.Once);
            _mapper.Verify(mapper => mapper.Map<List<EmployeeDto>>(employeesList), Times.Once);
            Assert.Equal(expectedEmployeeDtos, result);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_ExistingEmployeeId_ReturnsEmployeeDto()
        {
            // Arrange
            int employeeId = 1;
            var employeeToReturn = new Employee { Id = employeeId, FirstName = "John", LastName = "Doe" };

            _unitOfWork.Setup(uow => uow.EmployeeRepository.GetById(employeeId)).ReturnsAsync(employeeToReturn);

            var expectedEmployeeDto = new EmployeeDto { Id = employeeId, FirstName = "John", LastName = "Doe" };

            _mapper.Setup(mapper => mapper.Map<EmployeeDto>(employeeToReturn)).Returns(expectedEmployeeDto);

            // Act
            var result = await _employeeService.GetEmployeeByIdAsync(employeeId);

            // Assert
            _unitOfWork.Verify(repo => repo.EmployeeRepository.GetById(employeeId), Times.Once);
            _mapper.Verify(mapper => mapper.Map<EmployeeDto>(employeeToReturn), Times.Once);
            Assert.Equal(expectedEmployeeDto, result);
        }

        [Fact]
        public async Task GetEmployeeByIdAsync_NonExistingEmployeeId_ThrowsUserNotFoundException()
        {
            // Arrange
            int employeeId = 999; 
            _unitOfWork.Setup(uow => uow.EmployeeRepository.GetById(employeeId)).ReturnsAsync((Employee)null);

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _employeeService.GetEmployeeByIdAsync(employeeId));
        }

        [Fact]
        public async Task UpdateEmployeeAsync_ValidEmployeeDto_EmployeeUpdated()
        {
            // Arrange
            var employeeToUpdateDto = new EmployeeDto
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Email = "email@email.com",
                PhoneNumber = "+37063444123",
                DepartmentId = 1,
                PositionId = 1,
                Salary = 5000
            };

            var existingEmployee = new Employee
            {
                Id = 1,
                FirstName = "ExistingFirstName",
                LastName = "ExistingLastName",
                Email = "email@email.com",
                PhoneNumber = "+37063555666",
                DepartmentId = 1,
                PositionId = 1,
                Salary = 4000
            };

            _unitOfWork.Setup(uow => uow.EmployeeRepository.GetById(employeeToUpdateDto.Id)).ReturnsAsync(existingEmployee);


            // Act
            await _employeeService.UpdateEmployeeAsync(employeeToUpdateDto);

            // Assert
            Assert.Equal(employeeToUpdateDto.FirstName, existingEmployee.FirstName);
            Assert.Equal(employeeToUpdateDto.LastName, existingEmployee.LastName);
            Assert.Equal(employeeToUpdateDto.Email, existingEmployee.Email);
            Assert.Equal(employeeToUpdateDto.PhoneNumber, existingEmployee.PhoneNumber);
            Assert.Equal(employeeToUpdateDto.Salary, existingEmployee.Salary);

            _unitOfWork.Verify(repo => repo.EmployeeRepository.Update(existingEmployee), Times.Once);
            _unitOfWork.Verify(uow => uow.Commit(), Times.Once);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_NonExistingEmployee_ThrowsUserNotFoundException()
        {
            // Arrange
            var employeeToUpdateDto = new EmployeeDto
            {
                Id = 999,
                FirstName = "John",
                LastName = "Doe",
            };

            _unitOfWork.Setup(uow => uow.EmployeeRepository.GetById(employeeToUpdateDto.Id)).ReturnsAsync((Employee)null);

            // Act and Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _employeeService.UpdateEmployeeAsync(employeeToUpdateDto));
        }

        [Fact]
        public async Task UpdateEmployeeAsync_InvalidEmployeeDto_ThrowsInputValidationException()
        {
            // Arrange
            var employeeToUpdateDto = new EmployeeDto
            {
                Id = 1,
                FirstName = string.Empty,
                LastName = "Doe",
                Email = "email.com",
                PhoneNumber = "+8654",
                DepartmentId = 1,
                PositionId = 1,
                Salary = 2000
            };

            var existingEmployee = new Employee
            {
                Id = 1,
                FirstName = "ExistingFirstName",
                LastName = "ExistingLastName",
                Email = "email@email.com",
                PhoneNumber = "+37063444111",
                DepartmentId = 1,
                PositionId = 1,
                Salary = 2000
            };

            _unitOfWork.Setup(uow => uow.EmployeeRepository.GetById(employeeToUpdateDto.Id)).ReturnsAsync(existingEmployee);

            // Act and Assert
            await Assert.ThrowsAsync<InputValidationException>(() => _employeeService.UpdateEmployeeAsync(employeeToUpdateDto));
        }
    }
}
