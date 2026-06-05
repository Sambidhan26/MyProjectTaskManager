using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Common;
using TaskManager.API.Common.Pagination;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using TaskManager.API.Models.Common;
using TaskManager.API.Services.Interfaces;

namespace TaskManager.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(ICategoryService _categoryService) : Controller
    {
        [HttpGet]
        public async Task<ActionResult> GetAllCategories([FromQuery] PaginationParams pagination)
        {
            var categories = await _categoryService.GetAllCategoriesAsync(pagination);

            return Ok(new ApiResponse<IEnumerable<CategoryResponseDto>>
            {
                Success = true,
                Message = "Categories retrieved successfully",
                Data = categories
            });
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(new ApiResponse<CategoryResponseDto>
            {
                Success = true,
                Message = "Category retrieved successfully",
                Data = category
            });
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var category = await _categoryService.CreateCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, 
                new ApiResponse<CategoryResponseDto>
                {
                    Success = true,
                    Message = "Category created successfully",
                    Data = category
                });
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryResponseDto>> UpdateCategory(int id, [FromBody] UpadateCategoryDto dto)
        {
            var existingCategory = await _categoryService.UpdateCategoryAsync(id, dto);

            if (existingCategory == null)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Category not found"
                });
            }


            return Ok(new ApiResponse<CategoryResponseDto>
            {
                Success = true,
                Message = "Category updated successfully",
                Data = existingCategory
            });
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _categoryService.DeleteCategoryAsync(id);

            if (!existingCategory)
            {
                return NotFound(new ErrorResponse
                {
                    Message = "Category not found"
                });
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Category deleted successfully",
                Data = null
            });
        }
    }
}
