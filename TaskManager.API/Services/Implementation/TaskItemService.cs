using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Common.Pagination;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using TaskManager.API.Services.Interfaces;

namespace TaskManager.API.Services.Implementation
{
    public class TaskItemService(ApplicationDbContext _context, IMapper _mapper) : ITaskItemsService
    {
        public async Task<TaskItemReponseDto> CreateTaskAsync(CreateTaskItemDto dto, string userId)
        {
            var existingTask = _mapper.Map<TaskItem>(dto);

            existingTask.UserId = userId;
            existingTask.CreatedAt = DateTime.UtcNow;

            _context.TaskItems.Add(existingTask);

            await _context.SaveChangesAsync();

            return _mapper.Map<TaskItemReponseDto>(existingTask);



        }

        public async Task<bool> DeleteTaskAsync(int id, string userId)
        {
            var existingTask = _context.TaskItems.FirstOrDefault(t => t.Id == id && t.UserId == userId);
            if (existingTask == null)
            {
                return await Task.FromResult(false);
            }

            _context.TaskItems.Remove(existingTask);
            await _context.SaveChangesAsync();
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<TaskItemReponseDto>> GetAllTasksAsync(string userId, PaginationParams pagination)
        {
            var existingTask = await _context.TaskItems
                .Where(t => t.UserId == userId)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<TaskItemReponseDto>>(existingTask);
        }

        public async Task<TaskItemReponseDto> GetTaskByIdAsync(int id, string userId)
        {
            var existingTask = await _context.TaskItems
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .FirstOrDefaultAsync(t =>
                    t.Id == id &&
                    t.UserId == userId);

            if (existingTask == null)
            {
                return null;
            }

            return _mapper.Map<TaskItemReponseDto>(existingTask);
        }

        public async Task<TaskItemReponseDto> UpdateTaskAsync(int id, UpdateTaskItemDto dto, string userId)
        {
            var existingTask = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (existingTask == null)
            {
                return null;
            }

            _mapper.Map(dto, existingTask);


            await _context.SaveChangesAsync();

            return _mapper.Map<TaskItemReponseDto>(existingTask);
        }
            
    }
}
