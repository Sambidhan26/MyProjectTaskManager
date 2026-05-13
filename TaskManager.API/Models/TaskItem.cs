namespace TaskManager.API.Models
{
    public class TaskItem:BaseModel
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DueDate { get; set; }
        //Relationships Naviagation Properties
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public int PriorityId { get; set; }
        public Priority Priority { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
