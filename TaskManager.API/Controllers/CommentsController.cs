using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using TaskManager.API.Common;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using TaskManager.API.Models.Common;
using TaskManager.API.Services.Interfaces;

namespace TaskManager.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController(ICommentService _commentService) : ControllerBase
    {
        [HttpGet("{taskId:int}")]
        public async Task<ActionResult<CommentResponseDto>> GetCommentsByTaskId(int taskId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comment = await _commentService.GetCommentByIdAsync(taskId, userId!);

            if (comment == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Comment not found"
                });
            }

            return Ok(new ApiResponse<CommentResponseDto>
            {
                Success = true,
                Message = "Comment retrieved successfully",
                Data = comment
            });
        }

        [HttpGet("task/{taskId:int}")]
        public async Task<ActionResult<IEnumerable<CommentResponseDto>>> GetAllComments(int taskId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var comments = await _commentService.GetAllCommentsByTaskAsync(taskId,  userId!);
            return Ok(new ApiResponse<IEnumerable<CommentResponseDto>>
            {
                Success = true,
                Message = "Comments retrieved successfully",
                Data = comments
            });
        }

        [HttpPost("task/{taskId:int}")]
        public async Task<ActionResult<CommentResponseDto>> CreateComment([FromRoute] int taskId, [FromBody] CreateCommentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var comment = await _commentService.CreateCommentAsync(taskId,dto, userId!);
            if (comment == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Comment not found"
                });
            }


            return Ok(new ApiResponse<CommentResponseDto>
            {
                Success = true,
                Message = "Comment created successfully",
                Data = comment
            });
        }

        [HttpDelete("{taskIdd:int}")]
        public async Task<ActionResult> DeleteComment(int taskId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var comment = await _commentService.DeleteCommentAsync(taskId, userId!);

            if (!comment)
            {
                return NotFound("Comment not found");
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Comment deleted successfully",
                Data = null
            });
        }
    }
}
