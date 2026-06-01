using TaskManager.API.DTOs;

namespace TaskManager.API.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync();
    Task<CategoryResponseDto> GetCategoryByIdAsync(int id);
    Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto dto);
    Task<CategoryResponseDto> UpdateCategoryAsync(int id, UpadateCategoryDto dto);
    Task<bool> DeleteCategoryAsync(int id);
}
