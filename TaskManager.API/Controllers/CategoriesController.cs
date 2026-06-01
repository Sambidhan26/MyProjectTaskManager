using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using TaskManager.API.Services.Interfaces;

namespace TaskManager.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(ICategoryService _categoryService) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();

            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var category = await _categoryService.CreateCategoryAsync(dto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryResponseDto>> UpdateCategory(int id, [FromBody] UpadateCategoryDto dto)
        {
            var existingCategory = await _categoryService.UpdateCategoryAsync(id, dto);

            if (existingCategory == null)
            {
                return NotFound();
            }


            return Ok(existingCategory);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _categoryService.DeleteCategoryAsync(id);

            if (!existingCategory)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
