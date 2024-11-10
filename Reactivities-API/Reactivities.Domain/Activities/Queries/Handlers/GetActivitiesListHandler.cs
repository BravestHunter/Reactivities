﻿using AutoMapper;
using MediatR;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Interfaces;

namespace Reactivities.Domain.Activities.Queries.Handlers
{
    internal class GetFollowersListHandler : IRequestHandler<GetActivitiesListQuery, Result<PagedList<ActivityDto>>>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public GetFollowersListHandler(IActivityRepository activityRepository, IUserAccessor userAccessor, IMapper mapper)
        {
            _activityRepository = activityRepository;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<ActivityDto>>> Handle(GetActivitiesListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();
                var list = await _activityRepository.GetDtoList(request.PagingParams, request.Filters, currentUsername);

                return Result<PagedList<ActivityDto>>.Success(list);
            }
            catch (Exception ex)
            {
                return Result<PagedList<ActivityDto>>.Failure(ex);
            }
        }
    }
}
