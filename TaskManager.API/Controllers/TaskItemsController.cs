using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.API.Common;
using TaskManager.API.Common.Pagination;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.DTOs.StatsDto;
using TaskManager.API.Models;
using TaskManager.API.Models.Common;
using TaskManager.API.Services.Interfaces;

namespace TaskManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TaskItemsController(ITaskItemsService _taskService) : ControllerBase
    {

        //[HttpGet("test-error")]
        //public IActionResult TestError()
        //{
        //    throw new Exception("Test error");
        //}
        [HttpGet]
        public async Task<ActionResult> GetAllTaskItems([FromQuery] TaskFilterParams filter)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var taskItems = await _taskService.GetAllTasksAsync(userId!, filter);

            return Ok(new ApiResponse<IEnumerable<TaskItemReponseDto>>
            {
                Success = true,
                Message = "Task retrieved successfully",
                Data = taskItems
            });

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<TaskItemReponseDto>> GetTaskItemById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var taskItem = await _taskService.GetTaskByIdAsync(id, userId!);

            if (taskItem == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Task not found"
                });
            }

            return Ok(new ApiResponse<TaskItemReponseDto>
            {
                Success = true,
                Message = "Task retrieved successfully",
                Data = taskItem
            });
        }

        [HttpGet("stats")]
        public async Task<ActionResult<TaskStatsDto>> GetTaskStats()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var stats = await _taskService.GetTaskStatsAsync(userId!);
            return Ok(new ApiResponse<TaskStatsDto>
            {
                Success = true,
                Message = "Task stats retrieved successfully",
                Data = stats
            });
        }


        [HttpPost]
        public async Task<ActionResult<TaskItemReponseDto>> CreateTaskItem([FromBody] CreateTaskItemDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            dto.CreatedAt = DateTime.UtcNow;

            var createdTask = await _taskService.CreateTaskAsync(dto, userId!);

            return CreatedAtAction(nameof(GetTaskItemById),new { id = createdTask.Id }
                , new ApiResponse<TaskItemReponseDto>
                {
                    Success = true,
                    Message = "Task created successfully",
                    Data = createdTask
                });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<TaskItemReponseDto>> UpdateTaskItem(int id, [FromBody] UpdateTaskItemDto dto)
        {
            var useId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var updateTask = await _taskService.UpdateTaskAsync(id, dto, useId!);

            if(updateTask == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Task not found"
                });
            }


            return Ok(new ApiResponse<TaskItemReponseDto>
            {
                Success = true,
                Message = "Task updated successfully",
                Data = updateTask
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteTaskItem(int id)
        {
            var taskItem = await _taskService.DeleteTaskAsync(id, User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            if (!taskItem)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Task not found"
                });
            }


            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Task deleted successfully",
                Data = null
            });
        }
    }
}
