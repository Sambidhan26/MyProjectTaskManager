using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using TaskManager.API.Services.Interfaces;

namespace TaskManager.API.Services.Implementation
{
    public class TaskItemService(ApplicationDbContext _context) : ITaskItemsService
    {
        public async Task<TaskItemReponseDto> CreateTaskAsync(CreateTaskItemDto dto, string userId)
        {
            var existingTask = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                CreatedAt = DateTime.UtcNow,
                DueDate = dto.DueDate,
                CategoryId = dto.CategoryId,
                PriorityId = dto.PriorityId,
                UserId = userId
            };

            _context.TaskItems.Add(existingTask);

            await _context.SaveChangesAsync();

            return new TaskItemReponseDto
            {
                Id = existingTask.Id,
                Title = existingTask.Title,
                Description = existingTask.Description,
                IsCompleted = existingTask.IsCompleted,
                DueDate = existingTask.DueDate,
                CategoryId = existingTask.CategoryId,
                PriorityId = existingTask.PriorityId
            };



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

        public async Task<IEnumerable<TaskItemReponseDto>> GetAllTasksAsync(string userId)
        {
            var existingTask = await _context.TaskItems
                .Where(t => t.UserId == userId)
                .ToListAsync();

            return existingTask.Select(t => new TaskItemReponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                DueDate = t.DueDate,
                CategoryId = t.CategoryId,
                PriorityId = t.PriorityId
            });
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

            return new TaskItemReponseDto
            {
                Id = existingTask.Id,
                Title = existingTask.Title,
                Description = existingTask.Description,
                IsCompleted = existingTask.IsCompleted,
                DueDate = existingTask.DueDate,
                CategoryId = existingTask.CategoryId,
                CategoryName = existingTask.Category?.Name,
                PriorityId = existingTask.PriorityId,
                PriorityName = existingTask.Priority?.Level
            };
        }

        public async Task<TaskItemReponseDto> UpdateTaskAsync(int id, UpdateTaskItemDto dto, string userId)
        {
            var existingTask = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (existingTask == null)
            {
                return null;
            }

            existingTask.Title = dto.Title;
            existingTask.Description = dto.Description;
            existingTask.IsCompleted = dto.IsCompleted;
            existingTask.DueDate = dto.DueDate;
            existingTask.CategoryId = dto.CategoryId;
            existingTask.PriorityId = dto.PriorityId;


            await _context.SaveChangesAsync();

            return new TaskItemReponseDto
            {
                Id = existingTask.Id,
                Title = existingTask.Title,
                Description = existingTask.Description,
                IsCompleted = existingTask.IsCompleted,
                DueDate = existingTask.DueDate,
                CategoryId = existingTask.CategoryId,
                PriorityId = existingTask.PriorityId

            };
        }
            
    }
}
