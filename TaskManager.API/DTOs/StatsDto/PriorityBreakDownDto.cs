namespace TaskManager.API.DTOs.StatsDto
{
    public class PriorityBreakDownDto
    {
        public string PriorityName { get; set; } = string.Empty;

        public int TaskCount { get; set; }
    }
}
