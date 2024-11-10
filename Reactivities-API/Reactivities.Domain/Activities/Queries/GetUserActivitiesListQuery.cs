using MediatR;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Filters;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Activities.Queries
{
    public class GetUserActivitiesListQuery : IRequest<Result<PagedList<UserActivityDto>>>
    {
        public required PagingParams PagingParams { get; set; }
        public required UserActivityListFilters Filters { get; set; }
    }
}
