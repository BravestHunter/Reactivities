using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Activities.Models;

namespace Reactivities.Persistence.Repositories
{
    internal class ActivityRepository : IActivityRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ActivityRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Activity?> GetByIdAsync(long id)
        {
            return await _context.Activities.FindAsync(id);
        }

        public async Task<ActivityDto?> GetDtoByIdAsync(long id, string currentUsername)
        {
            return await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername })
                    .FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
