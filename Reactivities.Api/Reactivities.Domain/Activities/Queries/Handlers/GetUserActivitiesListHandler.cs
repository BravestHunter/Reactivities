using MediatR;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Filters;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;

namespace Reactivities.Domain.Activities.Queries.Handlers
{
    internal class GetUserActivitiesListHandler : IRequestHandler<GetUserActivitiesListQuery, Result<PagedList<UserActivityDto>>>
    {
        private readonly IActivityRepository _activityRepository;

        public GetUserActivitiesListHandler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
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
                return Result<PagedList<UserActivityDto>>.Failure(ex);
            }
        }
    }
}
