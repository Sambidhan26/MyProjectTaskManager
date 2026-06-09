using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManager.API.Common.Pagination;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.DTOs.StatsDto;
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

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategoriesAsync(PaginationParams pagination)
        {
            var existingCategory = await _context.Categories
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            return _mapper.Map<IEnumerable<CategoryResponseDto>>(existingCategory);


        }

        public async Task<CategoryResponseDto> GetCategoryByIdAsync(int id)
        {
            var existingCategory = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            return _mapper.Map<CategoryResponseDto>(existingCategory);
        }

        public async Task<IEnumerable<CategoryStatsBreakdown>> GetCategoryStatsBreakdownAsync(string userId)
        {
            var breakdown = await _context.TaskItems
                .Where(t => t.UserId == userId)
                .Include(t => t.Category)
                .GroupBy(t => t.Category!.Name)
                .Select(g => new CategoryStatsBreakdown
                {
                    CategoryName = g.Key!,
                    TaskCount = g.Count()
                }).ToListAsync();

            return breakdown;
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
