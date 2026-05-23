namespace TaskManager.API.DTOs
{
    public class CreateTaskItemDto
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public bool IsCompleted { get; set; }
        public DateTime? CreatedAt { get; set; }

        public DateTime? DueDate { get; set; }

        public int CategoryId { get; set; }

        public int? PriorityId { get; set; }
    }
}
