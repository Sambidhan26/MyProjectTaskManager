using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemsController(ApplicationDbContext _context) : ControllerBase
    {


        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemReponseDto>>> GetAllTaskItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var taskItems = await _context.TaskItems
                .Where(t => t.UserId == userId)
                .Select(u => new TaskItemReponseDto
                {
                    Id = u.Id,
                    Title = u.Title,
                    Description = u.Description,
                    IsCompleted = u.IsCompleted,
                    CreatedAt = u.CreatedAt,
                    DueDate = u.DueDate,
                    CategoryId = u.CategoryId,
                    CategoryName = u.Category != null ? u.Category.Name : null,
                    PriorityId = u.PriorityId,
                    PriorityName = u.Priority != null ? u.Priority.Level : null,
                })
                .ToListAsync();
            return Ok(taskItems);

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskItem>> GetTaskItemById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var taskItem = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (taskItem == null)
                return NotFound();

            var response = new TaskItemReponseDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                IsCompleted = taskItem.IsCompleted,
                CreatedAt = taskItem.CreatedAt,
                DueDate = taskItem.DueDate,
                CategoryId = taskItem.CategoryId,
                //CategoryName = taskItem.Category != null ? taskItem.Category.Name : null,
                PriorityId = taskItem.PriorityId,
                //PriorityName = taskItem.Priority != null ? taskItem.Priority.Level : null,
            };

            return Ok(response);
        }


        //[HttpPost]
        //public async Task<ActionResult<TaskItem>> CreateTaskItem([FromBody] TaskItem taskItem)
        //{
        //    taskItem.CreatedAt = DateTime.UtcNow;

        //    _context.TaskItems.Add(taskItem);
        //    await _context.SaveChangesAsync();

        //    return Ok(taskItem);
        //}

        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTaskItem([FromBody] CreateTaskItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (dto == null)
                return BadRequest("Invalid body");

            var taskItem = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                DueDate = dto.DueDate,
                CategoryId = dto.CategoryId,
                PriorityId = dto.PriorityId,
                CreatedAt = DateTime.UtcNow,
                UserId = userId!
            };

            _context.TaskItems.Add(taskItem);
            await _context.SaveChangesAsync();

            var response = new TaskItemReponseDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                IsCompleted = taskItem.IsCompleted,
                DueDate = taskItem.DueDate,
                CategoryId = taskItem.CategoryId,
                PriorityId = taskItem.PriorityId,
                CreatedAt = taskItem.CreatedAt
             };
            {

            }

            return Ok(response);

        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TaskItem>> UpdateTaskItem(int id, [FromBody] UpdateTaskItemDto dto)
        {
            var useId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var existingTaskItem = await _context.TaskItems
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == useId);

            if (existingTaskItem == null)
            {
                return NotFound();
            }

            existingTaskItem.Title = dto.Title;
            existingTaskItem.Description = dto.Description;
            existingTaskItem.IsCompleted = dto.IsCompleted;
            existingTaskItem.DueDate = dto.DueDate;
            existingTaskItem.CategoryId = dto.CategoryId;
            existingTaskItem.PriorityId = dto.PriorityId;

            await _context.SaveChangesAsync();

            var reponse = new TaskItemReponseDto
            {
                Id = existingTaskItem.Id,
                Title = existingTaskItem.Title,
                Description = existingTaskItem.Description,
                IsCompleted = existingTaskItem.IsCompleted,
                DueDate = existingTaskItem.DueDate,
                CategoryId = existingTaskItem.CategoryId,
                PriorityId = existingTaskItem.PriorityId,
            };


            return Ok(reponse);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTaskItem(int id)
        {
            var taskItem = await _context.TaskItems.FindAsync(id);
            if (taskItem == null)
            {
                return NotFound();
            }
            _context.TaskItems.Remove(taskItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
