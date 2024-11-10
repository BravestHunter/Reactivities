namespace Reactivities.Domain.Activities.Dtos
{
    public class EditActivityDto
    {
        public long Id { get; set; }
        public required string Title { get; set; }
        public DateTime Date { get; set; }
        public string? Description { get; set; }
        public required string Category { get; set; }
        public required string City { get; set; }
        public required string Venue { get; set; }
        public bool IsCancelled { get; set; }
    }
}
