namespace TaskManager.API.DTOs.StatsDto
{
    public class CategoryStatsBreakdown
    {
        public string CategoryName { get; set; } = null!;
        public int TaskCount { get; set; }
    }
}
