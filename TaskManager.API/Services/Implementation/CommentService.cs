using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Common.Pagination;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using TaskManager.API.Services.Interfaces;

namespace TaskManager.API.Services.Implementation
{
    public class CommentService(ApplicationDbContext _context, IMapper _mapper) : ICommentService
    {

        public async Task<CommentResponseDto> CreateCommentAsync(int taskId,CreateCommentDto dto, string userId)
        {
            var existingComment = await _context.TaskItems.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == userId);

            if(existingComment == null)
            {
                return null;
            }
            var comment = _mapper.Map<Comment>(dto);

            comment.TaskItemId = taskId;
            comment.CreatedAt = DateTime.UtcNow;


            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return _mapper.Map<CommentResponseDto>(comment);
        }

        public async Task<bool> DeleteCommentAsync(int commentId, string userId)
        {
            var existingComment = await _context.Comments
                .Include(c => c.TaskItem)
                .FirstOrDefaultAsync(c => c.Id == commentId && c.TaskItem!.UserId == userId);

            if (existingComment == null)
            {
                return false;
            }

            _context.Comments.Remove(existingComment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<CommentResponseDto>> GetAllCommentsByTaskAsync(int taskId, string userId)
        {
            var existingComments = await _context.TaskItems.AnyAsync(t => t.Id == taskId && t.UserId == userId);

            if(!existingComments)
            {
                return Enumerable.Empty<CommentResponseDto>();
            }

            var comment = await _context.Comments
                .Where(c => c.TaskItemId == taskId)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CommentResponseDto>>(comment);
        }

        public async Task<CommentResponseDto> GetCommentByIdAsync(int commentId, string userId)
        {
            var existingComment = await _context.TaskItems
                .Include(t => t.Comments)
                .FirstOrDefaultAsync(t => t.Id == commentId && t.UserId == userId);
            if(existingComment == null)
            {
                return null;
            }

            return  _mapper.Map<CommentResponseDto>(existingComment);
        }

    }
}
