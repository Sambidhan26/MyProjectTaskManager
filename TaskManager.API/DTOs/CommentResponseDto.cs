using TaskManager.API.Models;

namespace TaskManager.API.DTOs
{
    public class CommentResponseDto
    {
        public int Id { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        //Relationships Naviagation Properties
        public int? TaskItemId { get; set; }
        public TaskItem? TaskItem { get; set; }
    }
}
