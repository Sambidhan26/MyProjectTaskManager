using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTOs;
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
            var priorities = await _priorityService.GetAllPrioritiesAsync();
            return Ok(priorities);
        }

        [HttpGet("{priorityId:int}")]
        public async Task<ActionResult<PriorityResponseDto>> GetPriorityById(int priorityId)
        {
            var priority = await _priorityService.GetPriorityByIdAsync(priorityId);
            if (priority == null)
            {
                return NotFound("Priority not found");
            }
            return Ok(priority);
        }
    }
}
