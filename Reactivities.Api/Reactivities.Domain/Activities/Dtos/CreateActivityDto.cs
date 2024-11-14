namespace Reactivities.Domain.Activities.Dtos
{
    public class CreateActivityDto
    {
        public required string Title { get; set; }
        public required DateTime Date { get; set; }
        public string? Description { get; set; }
        public required string Category { get; set; }
        public required string City { get; set; }
        public required string Venue { get; set; }
    }
}
