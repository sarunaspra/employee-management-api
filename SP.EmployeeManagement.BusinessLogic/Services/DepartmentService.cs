﻿using AutoMapper;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<DepartmentService> _logger;

        public DepartmentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<DepartmentService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreateDepartmentAsync(DepartmentDto departmentDto)
        {
            await _unitOfWork.DepartmentRepository.Add(_mapper.Map<Department>(departmentDto));
            _logger.LogInformation($"Department - {departmentDto.Name}, was created at {DateTime.Now}");

            await _unitOfWork.Commit();
        }

        public async Task DeleteDepartmentAsync(int departmentId)
        {
            var departmentToDelete = await _unitOfWork.DepartmentRepository.GetById(departmentId);

            if (departmentToDelete is null)
            {
                _logger.LogInformation($"Department with id {departmentId} was not found");
                throw new UserNotFoundException();
            }

            _unitOfWork.DepartmentRepository.Delete(departmentToDelete);

            await _unitOfWork.Commit();
        }

        public async Task<DepartmentDto> GetDepartmentByIdAsync(int departmentId)
        {
            _logger.LogInformation($"Department with id {departmentId} was requested");
            var departmentToReturn = await _unitOfWork.DepartmentRepository.GetById(departmentId);

            if (departmentToReturn is null)
            {
                _logger.LogInformation($"Department with id {departmentId} was not found");
                throw new UserNotFoundException();
            }

            return _mapper.Map<DepartmentDto>(departmentToReturn);
        }

        public async Task<List<DepartmentDto>> GetDepartmentsAsync()
        {
            var departmentsList = await _unitOfWork.DepartmentRepository.GetAll();
            _logger.LogInformation($"List of {departmentsList.Count()} departments was returned");

            return _mapper.Map<List<DepartmentDto>>(departmentsList);
        }

        public async Task UpdateDepartmentAsync(DepartmentDto departmentToUpdateDto)
        {
            var department = await _unitOfWork.DepartmentRepository.GetById(departmentToUpdateDto.Id);

            if (department is null)
            {
                _logger.LogInformation($"Department with id {departmentToUpdateDto.Id} was not found");
                throw new UserNotFoundException();
            }

            department.Name = departmentToUpdateDto.Name;
            department.Description = departmentToUpdateDto.Description;

            _unitOfWork.DepartmentRepository.Update(department);
            _logger.LogInformation($"Information of department {department.Name} (id - {department.Id} was updated");

            await _unitOfWork.Commit();
        }
    }
}
