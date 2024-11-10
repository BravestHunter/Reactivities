namespace Reactivities.Domain.Activities.Filters
{
    public class ActivityListFilters
    {
        public DateTime FromDate { get; set; } = DateTime.UtcNow;
        public DateTime ToDate { get; set; } = DateTime.MaxValue;
        public ActivityRelationship Relationship { get; set; }
    }
}
