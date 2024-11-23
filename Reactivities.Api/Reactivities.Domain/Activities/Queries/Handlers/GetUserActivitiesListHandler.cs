using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Filters;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;

namespace Reactivities.Domain.Activities.Queries.Handlers
{
    internal sealed class GetUserActivitiesListHandler : IRequestHandler<GetUserActivitiesListQuery, Result<PagedList<UserActivityDto>>>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly ILogger _logger;

        public GetUserActivitiesListHandler(IActivityRepository activityRepository, ILogger<GetUserActivitiesListHandler> logger)
        {
            _activityRepository = activityRepository;
            _logger = logger;
        }

        public async Task<Result<PagedList<UserActivityDto>>> Handle(GetUserActivitiesListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.Filters.Relationship == ActivityRelationship.None)
                {
                    return Result<PagedList<UserActivityDto>>.Failure(new BadRequestException("Relationship type is not supported"));
                }

                var list = await _activityRepository.GetUserActivityDtoList(request.PagingParams, request.Filters);

                return Result<PagedList<UserActivityDto>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get user activities list");
                return Result<PagedList<UserActivityDto>>.Failure(ex);
            }
        }
    }
}
