using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(ApplicationDbContext _content) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAllCategories()
        {
            var categories = await _content.Categories
                .Include(u => u.TaskItems)
                .Select(u => new CategoryResponseDto
                {
                    Id = u.Id,
                    Name = u.Name
                })
                .ToListAsync();
            return Ok(categories);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Category>> GetCategoryById(int id)
        {
            var category = await _content.Categories
                .Include(u => u.TaskItems)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
                return NotFound();

            return Ok(category);
        }

        [HttpPost]
        public async Task<ActionResult<CategoryResponseDto>> CreateCategory([FromBody] CreateCategoryDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            _content.Categories.Add(category);
            await _content.SaveChangesAsync();

            var response = new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, response);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<CategoryResponseDto>> UpdateCategory(int id, [FromBody] UpadateCategoryDto dto)
        {
            var existingCategory = await _content.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = dto.Name ?? existingCategory.Name;
            await _content.SaveChangesAsync();

            var response = new CategoryResponseDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name
            };

            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var existingCategory = await _content.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            _content.Categories.Remove(existingCategory);
            await _content.SaveChangesAsync();

            return NoContent();
        }
    }
}
