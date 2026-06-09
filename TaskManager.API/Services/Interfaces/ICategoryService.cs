using TaskManager.API.Common.Pagination;
using TaskManager.API.DTOs;
using TaskManager.API.DTOs.StatsDto;

namespace TaskManager.API.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync(PaginationParams pagination);
    Task<CategoryResponseDto> GetCategoryByIdAsync(int id);
    Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task<CategoryResponseDto> UpdateCategoryAsync(int id, UpadateCategoryDto dto);
    Task<bool> DeleteCategoryAsync(int id);
    Task<IEnumerable<CategoryStatsBreakdown>> GetCategoryStatsBreakdownAsync(string userId);
}
