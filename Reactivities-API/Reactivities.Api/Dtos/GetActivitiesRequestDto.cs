using Microsoft.AspNetCore.Mvc.ModelBinding;
using Reactivities.Domain.Activities.Filters;

namespace Reactivities.Api.Dtos
{
    public class GetActivitiesRequestDto
    {
        [BindRequired]
        public required int PageNumber { get; set; }

        [BindRequired]
        public required int PageSize { get; set; }

        public DateTime FromDate { get; set; } = DateTime.UtcNow;
        public DateTime ToDate { get; set; } = DateTime.MaxValue;
        public ActivityRelationship Relationship { get; set; }
    }
}
