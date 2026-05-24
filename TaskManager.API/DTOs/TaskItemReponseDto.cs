namespace TaskManager.API.DTOs;

public class TaskItemReponseDto
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DueDate { get; set; }

    public int? CategoryId { get; set; }

    public string? CategoryName { get; set; }

    public int? PriorityId { get; set; }

    public string? PriorityName { get; set; }
}
