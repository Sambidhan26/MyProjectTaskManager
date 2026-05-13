namespace TaskManager.API.Models
{
    public class Category:BaseModel
    {
        public string? Name { get; set; }

        //Relationships Naviagation Properties
        public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }
}
