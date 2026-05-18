using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.Models;

namespace TaskManager.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(ApplicationDbContext _content) : Controller
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetAllCategories()
        {
            var categories = await _content.Categories
                .Include(u => u.TaskItems)
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
        public async Task<ActionResult<Category>> CreateCategory([FromBody] Category category)
        {
            _content.Categories.Add(category);
            await _content.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<Category>> UpdateCategory(int id, [FromBody] Category updatedCategory)
        {
            var existingCategory = await _content.Categories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.Name = updatedCategory.Name;

            await _content.SaveChangesAsync();

            return Ok(existingCategory);
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
