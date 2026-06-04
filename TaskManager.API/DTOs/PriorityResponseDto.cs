using TaskManager.API.Models;

namespace TaskManager.API.DTOs
{
    public class PriorityResponseDto
    {
        public string? Level { get; set; }

        //Relationships Naviagation Properties
        public ICollection<TaskItemPriorityDto> TaskItems { get; set; } = new List<TaskItemPriorityDto>();
    }
}
