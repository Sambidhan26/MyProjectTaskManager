using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManager.API.Common;
using TaskManager.API.DTOs;
using TaskManager.API.DTOs.StatsDto;
using TaskManager.API.Services.Interfaces;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriorityController(IPriorityService _priorityService) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PriorityResponseDto>>> GetAllPriorities()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var priorities = await _priorityService.GetAllPrioritiesAsync(userId!);
            return Ok(new ApiResponse<IEnumerable<PriorityResponseDto>>
            {
                Success = true,
                Message = "Priorities retrieved successfully",
                Data = priorities
            });
        }

        [HttpGet("{priorityId:int}")]
        public async Task<ActionResult<PriorityResponseDto>> GetPriorityById(int priorityId)
        {
            var priority = await _priorityService.GetPriorityByIdAsync(priorityId);
            if (priority == null)
            {
                return NotFound("Priority not found");
            }
            return Ok(new ApiResponse<PriorityResponseDto>
            {
                Success = true,
                Message = "Priority retrieved successfully",
                Data = priority
            });
        }

        [HttpGet("stats")]
        public async Task<ActionResult<PriorityStatsDto>> GetPriorityStats()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var stats = await _priorityService.GetPriorityStatsAsync(userId!);
            return Ok(new ApiResponse<PriorityStatsDto>
            {
                Success = true,
                Message = "Priority stats retrieved successfully",
                Data = stats
            });
        }

        [HttpGet("breakdown")]
        public async Task<ActionResult<IEnumerable<PriorityBreakDownDto>>> GetPriorityBreakDown()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var breakdown = await _priorityService.GetPriorityBreakDownAsync(userId!);
            return Ok(new ApiResponse<IEnumerable<PriorityBreakDownDto>>
            {
                Success = true,
                Message = "Priority breakdown retrieved successfully",
                Data = breakdown
            });
        }
    }
}
