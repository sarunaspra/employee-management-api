using AutoMapper;
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


        public EmployeeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateEmployeeAsync(EmployeeDto employeeDto)
        {
            await _unitOfWork.EmployeeRepository.Add(_mapper.Map<Employee>(employeeDto));

            await _unitOfWork.Commit();
        }

        public async Task DeleteEmployeeAsync(int employeeId)
        {
            var employeeToDelete = await _unitOfWork.EmployeeRepository.GetById(employeeId);

            if (employeeToDelete is null)
            {
                throw new UserNotFoundException();
            }

            _unitOfWork.EmployeeRepository.Delete(employeeToDelete);

            await _unitOfWork.Commit();
        }

        public async Task<List<EmployeeDto>> GetAllEmployeesAsync()
        {
            var employeesList = await _unitOfWork.EmployeeRepository.GetAll();

            return _mapper.Map<List<EmployeeDto>>(employeesList);
        }

        public async Task<EmployeeDto> GetEmployeeByIdAsync(int employeeId)
        {
            var employeeToReturn = await _unitOfWork.EmployeeRepository.GetById(employeeId);

            if (employeeToReturn is null)
            {
                throw new UserNotFoundException();
            }

            return _mapper.Map<EmployeeDto>(employeeToReturn);
        }

        public async Task UpdateEmployeeAsync(EmployeeDto employeeToUpdateDto)
        {
            var employee = await _unitOfWork.EmployeeRepository.GetById(employeeToUpdateDto.Id);

            if (employee is null)
            {
                throw new UserNotFoundException();
            }

            var employeeToUpdate = _mapper.Map<Employee>(employeeToUpdateDto);
            
            _unitOfWork.EmployeeRepository.Update(employeeToUpdate);

            await _unitOfWork.Commit();
        }
    }
}
