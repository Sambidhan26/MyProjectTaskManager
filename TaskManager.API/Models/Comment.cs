namespace TaskManager.API.Models;

public class Comment:BaseModel
{
    public string? Content { get; set; }
    public DateTime CreatedAt { get; set; }
    //Relationships Naviagation Properties
    public int? TaskItemId { get; set; }
    public TaskItem? TaskItem { get; set; }
}
