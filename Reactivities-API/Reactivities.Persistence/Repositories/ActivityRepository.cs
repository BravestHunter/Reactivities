using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Filters;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Core;
using Reactivities.Persistence.Extensions;

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

        public async Task<Activity?> GetById(long id)
        {
            return await _context.Activities.FindAsync(id);
        }

        public async Task<Activity?> GetByIdWithHost(long id)
        {
            return await _context.Activities
                .Include(a => a.Host)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Activity?> GetByIdWithAttendees(long id)
        {
            return await _context.Activities
                .Include(a => a.Host)
                .Include(a => a.Attendees)
                .ThenInclude(aa => aa.User)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<ActivityDto?> GetDtoById(long id, string currentUsername)
        {
            return await _context.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername })
                    .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<PagedList<ActivityDto>> GetDtoList(PagingParams pagingParams, ActivityListFilters filters, string currentUsername)
        {
            var query = _context.Activities
                .Where(a => a.Date >= filters.StartDate)
                .OrderBy(a => a.Date)
                .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername })
                .AsQueryable();

            switch (filters.Relationship)
            {
                case ActivityRelationship.None:
                    break;

                case ActivityRelationship.IsHost:
                    query = query.Where(a => a.Host.Username == currentUsername);
                    break;

                case ActivityRelationship.IsGoing:
                    query = query.Where(a => a.Attendees.Any(u => u.Username == currentUsername));
                    break;
            }

            return await query.ToPagedList(pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<PagedList<UserActivityDto>> GetUserActivityDtoList(PagingParams pagingParams, UserActivityListFilters filters)
        {
            var query = _context.Activities
                .Where(a => a.Date >= filters.StartDate);

            switch (filters.Relationship)
            {
                case ActivityRelationship.None:
                    break;

                case ActivityRelationship.IsHost:
                    query = query.Where(a => a.Host.UserName == filters.TargetUsername);
                    break;

                case ActivityRelationship.IsGoing:
                    query = query.Where(a => a.Attendees.Any(u => u.User.UserName == filters.TargetUsername));
                    break;
            }

            var dtoQuery = query
                .OrderBy(a => a.Date)
                .ProjectTo<UserActivityDto>(_mapper.ConfigurationProvider)
                .AsQueryable();

            return await dtoQuery.ToPagedList(pagingParams.PageNumber, pagingParams.PageSize);
        }

        public async Task<ActivityDto> Add(Activity activity)
        {
            await _context.Activities.AddAsync(activity);
            await SaveChanges();

            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<ActivityDto> Update(Activity activity)
        {
            _context.Activities.Update(activity);
            await SaveChanges();

            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task Delete(Activity activity)
        {
            _context.Activities.Remove(activity);
            await SaveChanges();
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
