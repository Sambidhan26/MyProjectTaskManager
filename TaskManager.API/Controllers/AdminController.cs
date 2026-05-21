using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminController(ApplicationDbContext _context, UserManager<IdentityUser> _userManager) : ControllerBase
    {
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var users = _userManager.Users.ToList();

            return Ok(users);
        }

        [HttpGet("tasks")]
        public async Task<IActionResult> GetAllTasks()
        {
            var tasks = await _context.TaskItems
                .Include(t => t.Category)
                .Include(t => t.Priority)
                .ToListAsync();

            return Ok(tasks);
        }
        [HttpDelete("tasks/{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var task = await _context.TaskItems.FindAsync(id);

            if (task == null)
                return NotFound();

            _context.TaskItems.Remove(task);

            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
