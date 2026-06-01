using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using TaskManager.API.Services.Interfaces;

namespace TaskManager.API.Services.Implementation
{
    public class CategoryService(ApplicationDbContext _context, IMapper _mapper) : ICategoryService
    {
        public async Task<CategoryResponseDto> CreateCategoryAsync(CreateCategoryDto dto)
        {
            var existingCategory =  _mapper.Map<Category>(dto);

            if (existingCategory == null)
            {
                return null;
            }

            _context.Categories.Add(existingCategory);

            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryResponseDto>(existingCategory);
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (existingCategory == null)
            {
                return false;
            }
            _context.Categories.Remove(existingCategory);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync()
        {
            var existingCategory = await _context.Categories.ToListAsync();

            return _mapper.Map<IEnumerable<CategoryResponseDto>>(existingCategory);


        }

        public async Task<CategoryResponseDto> GetCategoryByIdAsync(int id)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<CategoryResponseDto>(existingCategory);
        }

        public async Task<CategoryResponseDto> UpdateCategoryAsync(int id, UpadateCategoryDto dto)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (existingCategory == null)
            {
                return null;
            }
            _mapper.Map(dto, existingCategory);
            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();

            return _mapper.Map<CategoryResponseDto>(existingCategory);
        }
    }
}
