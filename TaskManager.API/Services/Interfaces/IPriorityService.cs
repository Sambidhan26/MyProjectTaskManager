using TaskManager.API.DTOs;
using TaskManager.API.DTOs.StatsDto;

namespace TaskManager.API.Services.Interfaces
{
    public interface IPriorityService
    {
        public Task<IEnumerable<PriorityResponseDto>> GetAllPrioritiesAsync(string userId);
        public Task<PriorityResponseDto> GetPriorityByIdAsync(int priorityId);

        public Task<PriorityStatsDto> GetPriorityStatsAsync(string userId);
    }
}
