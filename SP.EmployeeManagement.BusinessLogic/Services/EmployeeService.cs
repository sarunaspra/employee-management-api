using AutoMapper;
using SP.EmployeeManagement.BusinessLogic.Services.IServices;
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

        public async Task CreateEmployee(EmployeeDto employee)
        {
            if (employee is not null)
            {
                await _unitOfWork.EmployeeRepository.Add(_mapper.Map<Employee>(employee));

                _unitOfWork.SaveChanges();
            }
        }

        public async Task DeleteEmployee(int employeeId)
        {
            var employeeToDelete = await _unitOfWork.EmployeeRepository.GetById(employeeId);

            _unitOfWork.EmployeeRepository.Delete(employeeToDelete);

            _unitOfWork.SaveChanges();
        }

        public async Task<List<EmployeeDto>> GetAllEmployees()
        {
            var employeesList = await _unitOfWork.EmployeeRepository.GetAll();
            return _mapper.Map<List<EmployeeDto>>(employeesList);
        }

        public async Task<EmployeeDto> GetEmployeeById(int employeeId)
        {
            var employeeToReturn = await _unitOfWork.EmployeeRepository.GetById(employeeId);

            return _mapper.Map<EmployeeDto>(employeeToReturn);
        }

        public async Task UpdateEmployee(EmployeeDto employeeToUpdateDto)
        {
            var employeeToUpdate = _mapper.Map<Employee>(employeeToUpdateDto);

            _unitOfWork.EmployeeRepository.Update(employeeToUpdate);

            _unitOfWork.SaveChanges();
        }
    }
}
