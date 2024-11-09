using MediatR;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Filters;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Activities.Queries
{
    public class GetActivityListQuery : IRequest<Result<PagedList<ActivityDto>>>
    {
        public required PagingParams PagingParams { get; set; }
        public required ActivityListFilters Filters { get; set; }
    }
}
