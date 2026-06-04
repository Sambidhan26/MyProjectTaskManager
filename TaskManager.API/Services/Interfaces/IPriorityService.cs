using TaskManager.API.DTOs;

namespace TaskManager.API.Services.Interfaces
{
    public interface IPriorityService
    {
        public Task<IEnumerable<PriorityResponseDto>> GetAllPrioritiesAsync();
        public Task<PriorityResponseDto> GetPriorityByIdAsync(int priorityId);
    }
}
