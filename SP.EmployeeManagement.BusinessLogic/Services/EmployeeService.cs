using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using SP.EmployeeManagement.BusinessLogic.Services.IServices;
using SP.EmployeeManagement.BusinessLogic.Utilities.CustomExceptions;
using SP.EmployeeManagement.DataAccess.Entities;
using SP.EmployeeManagement.DataAccess.Interfaces;
using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.BusinessLogic.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<EmployeeService> _logger;
        private readonly IValidator<EmployeeDto> _employeeDtoValidator;

        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<EmployeeService> logger, IValidator<EmployeeDto> employeeDtoValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _employeeDtoValidator = employeeDtoValidator;
        }

        public async Task CreateEmployeeAsync(EmployeeDto employeeDto)
        {
            var validationResult = await _employeeDtoValidator.ValidateAsync(employeeDto);

            if (validationResult.IsValid)
            {
                await _unitOfWork.EmployeeRepository.Add(_mapper.Map<Employee>(employeeDto));
                _logger.LogInformation($"Employee {employeeDto.FirstName} {employeeDto.LastName} was created");

                await _unitOfWork.Commit();
            }
            else
            {
                throw new InputValidationException(validationResult.Errors.ToList());
            }
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            var employeeToDelete = await _unitOfWork.EmployeeRepository.GetById(employeeId);

            if (employeeToDelete is null)
            {
                _logger.LogInformation($"Employee with id {employeeId} was not found");
                throw new UserNotFoundException();
            }

            _unitOfWork.EmployeeRepository.Delete(employeeToDelete);

            await _unitOfWork.Commit();
        }

        public async Task<List<EmployeeDto>> GetEmployeesAsync()
        {
            var employeesList = await _unitOfWork.EmployeeRepository.GetAll();
            _logger.LogInformation($"List of {employeesList.Count()} employees was returned");

            return _mapper.Map<List<EmployeeDto>>(employeesList);
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int employeeId)
        {
            _logger.LogInformation($"Employee with id {employeeId} was requested");
            var employeeToReturn = await _unitOfWork.EmployeeRepository.GetById(employeeId);

            if (employeeToReturn is null)
            {
                _logger.LogInformation($"Employee with id {employeeId} was not found");
                throw new UserNotFoundException();
            }

            return _mapper.Map<EmployeeDto>(employeeToReturn);
        }

        public async Task UpdateEmployeeAsync(EmployeeDto employeeToUpdateDto)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetById(employeeToUpdateDto.Id);

            if (employee is null)
            {
                _logger.LogInformation($"Employee with id {employeeToUpdateDto.Id} was not found");
                throw new UserNotFoundException();
            }

            var validationResult = await _employeeDtoValidator.ValidateAsync(employeeToUpdateDto);

            if (validationResult.IsValid)
            {
                employee.FirstName = employeeToUpdateDto.FirstName;
                employee.LastName = employeeToUpdateDto.LastName;
                employee.Email = employeeToUpdateDto.Email;
                employee.PhoneNumber = employeeToUpdateDto.PhoneNumber;
                employee.DepartmentId = employeeToUpdateDto.DepartmentId;
                employee.PositionId = employeeToUpdateDto.PositionId;
                employee.Salary = employeeToUpdateDto.Salary;
            
                _unitOfWork.EmployeeRepository.Update(employee);
                _logger.LogInformation($"Information of employee {employee.FirstName} {employee.LastName} (id - {employee.Id}) was updated");

                await _unitOfWork.Commit();
            }
            else
            {
                throw new InputValidationException(validationResult.Errors.ToList());
            }
        }
    }
}
