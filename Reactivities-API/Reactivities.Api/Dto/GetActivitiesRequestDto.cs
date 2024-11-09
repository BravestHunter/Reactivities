using Microsoft.AspNetCore.Mvc.ModelBinding;
using Reactivities.Domain.Activities.Filters;

namespace Reactivities.Api.Dto
{
    public class GetActivitiesRequestDto
    {
        [BindRequired]
        public required int PageNumber { get; set; }

        [BindRequired]
        public required int PageSize { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public ActivityRelationship Relationship { get; set; }
    }
}
