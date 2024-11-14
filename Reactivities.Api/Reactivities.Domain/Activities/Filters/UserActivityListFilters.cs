namespace Reactivities.Domain.Activities.Filters
{
    public class UserActivityListFilters : ActivityListFilters
    {
        public required string TargetUsername { get; set; }
    }
}
