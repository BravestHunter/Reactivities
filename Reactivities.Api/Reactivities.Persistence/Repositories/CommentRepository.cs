using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain.Comments.Dtos;
using Reactivities.Domain.Comments.Interfaces;

namespace Reactivities.Persistence.Repositories
{
    internal sealed class CommentRepository : ICommentRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CommentRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<CommentDto>> GetDtoList(long activityId)
        {
            return await _context.Comments
                    .Where(c => c.Activity.Id == activityId)
                    .OrderByDescending(c => c.CreatedAt)
                    .ProjectTo<CommentDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
        }

        private async Task SaveChanges()
        {
            var result = await _context.SaveChangesAsync() > 0;
            if (!result)
            {
                throw new InvalidOperationException("Failed to save persistent changes");
            }
        }
    }
}
