namespace Reactivities.Domain.Activities.Dtos
{
    public class UserActivityDto
    {
        public required long Id { get; set; }
        public required string Title { get; set; }
        public required string Category { get; set; }
        public required DateTime Date { get; set; }
    }
}
