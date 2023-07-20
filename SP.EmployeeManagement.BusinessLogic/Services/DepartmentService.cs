using AutoMapper;
using SP.EmployeeManagement.BusinessLogic.Services.IServices;
using SP.EmployeeManagement.BusinessLogic.Utilities.CustomExceptions;
using SP.EmployeeManagement.DataAccess.Entities;
using SP.EmployeeManagement.DataAccess.Interfaces;
using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.BusinessLogic.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateDepartmentAsync(DepartmentDto departmentDto)
        {
            await _unitOfWork.DepartmentRepository.Add(_mapper.Map<Department>(departmentDto));

            await _unitOfWork.Commit();
        }

        public async Task DeleteDepartmentAsync(int departmentId)
        {
            var departmentToDelete = await _unitOfWork.DepartmentRepository.GetById(departmentId);

            if (departmentToDelete is null)
            {
                throw new UserNotFoundException();
            }

            _unitOfWork.DepartmentRepository.Delete(departmentToDelete);

            await _unitOfWork.Commit();
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId)
        {
            var departmentToReturn = await _unitOfWork.DepartmentRepository.GetById(departmentId);

            if (departmentToReturn is null)
            {
                throw new UserNotFoundException();
            }

            return _mapper.Map<DepartmentDto>(departmentToReturn);
        }

        public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        {
            var departmentsList = await _unitOfWork.DepartmentRepository.GetAll();

            return _mapper.Map<List<DepartmentDto>>(departmentsList);
        }

        public async Task UpdateDepartmentAsync(DepartmentDto departmentToUpdateDto)
        {
            var department = await _unitOfWork.DepartmentRepository.GetById(departmentToUpdateDto.Id);

            if (department is null)
            {
                throw new UserNotFoundException();
            }

            department.Name = departmentToUpdateDto.Name;
            department.Description = departmentToUpdateDto.Description;

            _unitOfWork.DepartmentRepository.Update(department);

            await _unitOfWork.Commit();
        }
    }
}
