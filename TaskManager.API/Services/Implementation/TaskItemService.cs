using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Common;
using TaskManager.API.Common.Pagination;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.DTOs.StatsDto;
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

        public async Task<IEnumerable<TaskItemReponseDto>> GetAllTasksAsync(string userId, TaskFilterParams filter)
        {
            var query = _context.TaskItems
                .Where(t => t.UserId == userId)
                .AsQueryable();

            if (filter.IsCompleted.HasValue)
            {
                query = query.Where(t => t.IsCompleted == filter.IsCompleted.Value);
            }
            if (filter.CategoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == filter.CategoryId.Value);
            }
            if (filter.PriorityId.HasValue)
            {
                query = query.Where(t => t.PriorityId == filter.PriorityId.Value);
            }

            if(!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.ToLower();

                query = query.Where(t =>
                    (t.Title != null && t.Title.ToLower().Contains(search)) ||
                    (t.Description != null && t.Description.ToLower().Contains(search)));
            }

            if(!string.IsNullOrEmpty(filter.SortBy))
            {
                query = filter.SortBy.ToLower() switch
                {
                    "title" => filter.Descending
                        ? query.OrderByDescending(t => t.Title)
                        : query.OrderBy(t => t.Title),

                    "createdat" => filter.Descending
                        ? query.OrderByDescending(t => t.CreatedAt)
                        : query.OrderBy(t => t.CreatedAt),

                    "duedate" => filter.Descending
                        ? query.OrderByDescending(t => t.DueDate)
                        : query.OrderBy(t => t.DueDate),

                    _ => query.OrderByDescending(t => t.CreatedAt)
                };
            }

            query = query.OrderByDescending(t => t.CreatedAt);

            var existingTask = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
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

        //Stats
        public async Task<TaskStatsDto> GetTaskStatsAsync(string userId)
        {
            var totalTasks = await _context.TaskItems.CountAsync(t => t.UserId == userId);
            var completedTasks = await _context.TaskItems.CountAsync(t => t.UserId == userId && t.IsCompleted);
            var pendingTasks = totalTasks - completedTasks;

            return new TaskStatsDto
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                PendingTasks = pendingTasks
            };
        }

    }
}
