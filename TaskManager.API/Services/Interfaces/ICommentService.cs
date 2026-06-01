using TaskManager.API.DTOs;

namespace TaskManager.API.Services.Interfaces;

public interface ICommentService
{
    public Task<IEnumerable<CommentResponseDto>> GetAllCommentsByTaskAsync(int taskId, string userId);
    public Task<CommentResponseDto> GetCommentByIdAsync(int taskId, string userId);
    public Task<CommentResponseDto> CreateCommentAsync(int taskId,CreateCommentDto dto, string userId);
    public Task<bool> DeleteCommentAsync(int commentId, string userId);
}
