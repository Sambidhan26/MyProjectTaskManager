using TaskManager.API.Common.Pagination;

namespace TaskManager.API.Common
{
    public class TaskFilterParams:PaginationParams
    {
        public bool? IsCompleted { get; set; }

        public int? CategoryId { get; set; }

        public int? PriorityId { get; set; }

        public string? SortBy { get; set; }

        public bool Descending { get; set; } = false;

        public string? Search { get; set; }
    }
}
