using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManager.API.Data;
using TaskManager.API.DTOs;
using TaskManager.API.Models;
using TaskManager.API.Services.Interfaces;

namespace TaskManager.API.Services.Implementation
{
    public class PriorityService(ApplicationDbContext _context, IMapper _mapper):IPriorityService
    {
        public async Task<IEnumerable<PriorityResponseDto>> GetAllPrioritiesAsync()
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
    }
}
