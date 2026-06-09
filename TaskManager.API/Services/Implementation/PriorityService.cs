using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.DTOs.StatsDto;
using TaskManager.API.Models;
using TaskManager.API.Services.Interfaces;

namespace TaskManager.API.Services.Implementation
{
    public class PriorityService(ApplicationDbContext _context, IMapper _mapper):IPriorityService
    {
        public async Task<IEnumerable<PriorityResponseDto>> GetAllPrioritiesAsync(string userId)
        {
            var priorities = await _context.Priorities
                .Include(p => p.TaskItems)
                .ToListAsync();
            return _mapper.Map<IEnumerable<PriorityResponseDto>>(priorities);
        }

        public async Task<PriorityResponseDto> GetPriorityByIdAsync(int priorityId)
        {
            var priority = await _context.Priorities
                .Include (p => p.TaskItems)
                .FirstOrDefaultAsync(p => p.Id == priorityId);
            if (priority == null)
            {
                return null;
            }
            return _mapper.Map<PriorityResponseDto>(priority);
        }

        public async Task<PriorityStatsDto> GetPriorityStatsAsync(string userId)
        {
            var tasks = _context.TaskItems.Where(t => t.UserId == userId);

            return new PriorityStatsDto
            {
                High = await tasks.CountAsync(t => t.Priority!.Level == "High"),
                Medium = await tasks.CountAsync(t => t.Priority!.Level == "Medium"),
                Low = await tasks.CountAsync(t => t.Priority!.Level == "Low")
            };
        }

        public async Task<IEnumerable<PriorityBreakDownDto>> GetPriorityBreakDownAsync(string userId)
        {
            var breakdown = await _context.TaskItems
                .Where(t => t.UserId == userId)
                .Include(t => t.Priority)
                .GroupBy(t => t.Priority!.Level)
                .Select(g => new PriorityBreakDownDto
                {
                    PriorityName = g.Key!,
                    TaskCount = g.Count()
                }).ToListAsync();

            return breakdown;
        }
    }
}
