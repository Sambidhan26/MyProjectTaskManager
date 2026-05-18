using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController(ApplicationDbContext _context) : ControllerBase
    {
        [HttpGet("task/{id:int}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetCommentsByTaskId(int id)
        {
            var comments = await _context.Comments
                .Where(c => c.Id == id)
                .Select(c => new CommentResponseDto
                {
                    Id = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    TaskItemId = c.TaskItemId
                })
                .ToListAsync();

            return Ok(comments);
        }

        [HttpPost("task/{id:int}")]
        public async Task<ActionResult<CommentResponseDto>> CreateComment(int id, [FromBody] CreateCommentDto dto)
        {
            // Check if task exists
            var taskExists = await _context.TaskItems.AnyAsync(t => t.Id == id);

            if (!taskExists)
            {
                return NotFound("Task not found");
            }

            var comment = new Comment
            {
                Content = dto.Content,
                CreatedAt = DateTime.UtcNow,
                TaskItemId = id
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            var response = new CommentResponseDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                TaskItemId = comment.TaskItemId
            };

            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Comment deleted successfully" });
        }
    }
}
