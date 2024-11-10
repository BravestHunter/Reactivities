﻿using AutoMapper;
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

        public async Task<ActivityDto> Add(Activity activity)
        {
            await _context.Activities.AddAsync(activity);

            await Save();

            return _mapper.Map<ActivityDto>(activity);
        }

        public async Task<ActivityDto> Update(Activity activity)
        {
            _context.Activities.Update(activity);

            await Save();

            return _mapper.Map<ActivityDto>(activity);
        }

        private async Task Save()
        {
            var result = await _context.SaveChangesAsync() > 0;
            if (!result)
            {
                throw new InvalidOperationException("Failed to save persistent change");
            }
        }
    }
}