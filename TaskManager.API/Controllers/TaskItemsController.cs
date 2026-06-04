using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using TaskManager.API.Services.Interfaces;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemsController(ITaskItemsService _taskService) : ControllerBase
    {

        [HttpGet("test-error")]
        public IActionResult TestError()
        {
            throw new Exception("Test error");
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TaskItemReponseDto>>> GetAllTaskItems()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var taskItems = await _taskService.GetAllTasksAsync(userId!);
            return Ok(taskItems);

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskItemReponseDto>> GetTaskItemById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var taskItem = await _taskService.GetTaskByIdAsync(id, userId!);

            if (taskItem == null)
            {
                return NotFound();
            }

            return Ok(taskItem);
        }


        [HttpPost]
        public async Task<ActionResult<TaskItem>> CreateTaskItem([FromBody] CreateTaskItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            dto.CreatedAt = DateTime.UtcNow;

            var createdTask = await _taskService.CreateTaskAsync(dto, userId!);

            return CreatedAtAction(nameof(GetTaskItemById),new { id = createdTask.Id },createdTask);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TaskItem>> UpdateTaskItem(int id, [FromBody] UpdateTaskItemDto dto)
        {
            var useId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var updateTask = await _taskService.UpdateTaskAsync(id, dto, useId!);

            if(updateTask == null)
            {
                return NotFound();
            }


            return Ok(updateTask);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTaskItem(int id)
        {
            var taskItem = await _taskService.DeleteTaskAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!taskItem)
            {
                return NotFound();
            }


            return NoContent();
        }
    }
}
