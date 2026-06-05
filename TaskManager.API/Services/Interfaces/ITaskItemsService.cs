using TaskManager.API.Common.Pagination;
using TaskManager.API.DTOs;

namespace TaskManager.API.Services.Interfaces
{
    public interface ITaskItemsService
    {
        Task<IEnumerable<TaskItemReponseDto>> GetAllTasksAsync(string userId, PaginationParams pagination);
        Task<TaskItemReponseDto> GetTaskByIdAsync(int id, string userId);
        Task<TaskItemReponseDto> CreateTaskAsync(CreateTaskItemDto item, string userId);
        Task<TaskItemReponseDto> UpdateTaskAsync(int id, UpdateTaskItemDto item, string userId);
        Task<bool> DeleteTaskAsync(int id, string userId);
    }
}
