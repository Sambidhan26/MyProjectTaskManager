namespace TaskManager.API.Models
{
    public class Priority:BaseModel
    {
        public string? Level { get; set; }

        //Relationships Naviagation Properties
        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }
}
