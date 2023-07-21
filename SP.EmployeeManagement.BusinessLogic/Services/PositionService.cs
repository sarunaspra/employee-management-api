using AutoMapper;
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

        public PositionService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PositionService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task CreatePositionAsync(PositionDto positionDto)
        {
            await _unitOfWork.PositionRepository.Add(_mapper.Map<Position>(positionDto));
            _logger.LogInformation($"Position {positionDto.Title} was created at {DateTime.Now}");

            await _unitOfWork.Commit();
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

            position.Title = positionToUpdateDto.Title;
            position.Description = positionToUpdateDto.Description;

            _unitOfWork.PositionRepository.Update(position);
            _logger.LogInformation($"Information of position {position.Title} (id - {position.Id}) was updated");

            await _unitOfWork.Commit();
        }
    }
}
