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
    public class PositionService : IPositionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PositionService> _logger;
        private readonly IValidator<PositionDto> _positionDtoValidator;

        public PositionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PositionService> logger, IValidator<PositionDto> positionDtoValidator)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _positionDtoValidator = positionDtoValidator;
        }

        public async Task CreatePositionAsync(PositionDto positionDto)
        {
            var validationResult = await _positionDtoValidator.ValidateAsync(positionDto);

            if (validationResult.IsValid)
            {
                await _unitOfWork.PositionRepository.Add(_mapper.Map<Position>(positionDto));
                _logger.LogInformation($"Position {positionDto.Title} was created at");

                await _unitOfWork.Commit();
            }
            else
            {
                throw new InputValidationException(validationResult.Errors.ToList());
            }
        }

        public async Task DeletePositionAsync(int positionId)
        {
            var positionToDelete = await _unitOfWork.PositionRepository.GetById(positionId);

            if (positionToDelete is null)
            {
                _logger.LogInformation($"Position with id {positionId} was not found");
                throw new UserNotFoundException();
            }

            _unitOfWork.PositionRepository.Delete(positionToDelete);

            await _unitOfWork.Commit();
        }

        public async Task<PositionDto> GetPositionByIdAsync(int positionId)
        {
            _logger.LogInformation($"Position with id {positionId} was requested");
            var positionToReturn = await _unitOfWork.PositionRepository.GetById(positionId);

            if (positionToReturn is null)
            {
                _logger.LogInformation($"Position with id {positionId} was not found");
                throw new UserNotFoundException();
            }

            return _mapper.Map<PositionDto>(positionToReturn);
        }

        public async Task<List<PositionDto>> GetPositionsAsync()
        {
            var positionsList = await _unitOfWork.PositionRepository.GetAll();
            _logger.LogInformation($"List of {positionsList.Count()} positions was returned");

            return _mapper.Map<List<PositionDto>>(positionsList);
        }

        public async Task UpdatePositionAsync(PositionDto positionToUpdateDto)
        {
            var position = await _unitOfWork.PositionRepository.GetById(positionToUpdateDto.Id);

            if (position is null)
            {
                _logger.LogInformation($"Position with id {positionToUpdateDto.Id} was not found");
                throw new UserNotFoundException();
            }

            var validationResult = await _positionDtoValidator.ValidateAsync(positionToUpdateDto);

            if (validationResult.IsValid)
            {
                position.Title = positionToUpdateDto.Title;
                position.Description = positionToUpdateDto.Description;

                _unitOfWork.PositionRepository.Update(position);
                _logger.LogInformation($"Information of position {position.Title} (id - {position.Id}) was updated");

                await _unitOfWork.Commit();
            }
            else
            {
                throw new InputValidationException(validationResult.Errors.ToList());
            }
        }
    }
}
