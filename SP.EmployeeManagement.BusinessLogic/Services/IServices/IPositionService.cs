using SP.EmployeeManagement.Dto.Dtos;

namespace SP.EmployeeManagement.BusinessLogic.Services.IServices
{
    public interface IPositionService
    {
        Task CreatePositionAsync(PositionDto positionDto);

        Task<List<PositionDto>> GetPositionsAsync();

        Task<PositionDto> GetPositionByIdAsync(int positionId);

        Task UpdatePositionAsync(PositionDto positionToUpdateDto);

        Task DeletePositionAsync(int positionId);
    }
}
