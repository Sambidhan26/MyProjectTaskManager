using TaskManager.API.Common;
using TaskManager.API.Common.Pagination;
using TaskManager.API.DTOs;
using TaskManager.API.DTOs.StatsDto;

namespace TaskManager.API.Services.Interfaces
{
    public interface ITaskItemsService
    {
        Task<IEnumerable<TaskItemReponseDto>> GetAllTasksAsync(string userId, TaskFilterParams filter);
        Task<TaskItemReponseDto> GetTaskByIdAsync(int id, string userId);
        Task<TaskItemReponseDto> CreateTaskAsync(CreateTaskItemDto item, string userId);
        Task<TaskItemReponseDto> UpdateTaskAsync(int id, UpdateTaskItemDto item, string userId);
        Task<bool> DeleteTaskAsync(int id, string userId);
        Task<TaskStatsDto> GetTaskStatsAsync(string userId);
    }
}
