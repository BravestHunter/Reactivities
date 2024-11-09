namespace Reactivities.Domain.Activities.Filters
{
    public class ActivityListFilters
    {
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public ActivityRelationship Relationship { get; set; }
    }
}
