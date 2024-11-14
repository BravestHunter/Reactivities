namespace Reactivities.Domain.Activities.Dtos
{
    public class ActivityDto
    {
        public required long Id { get; set; }
        public required string Title { get; set; }
        public required DateTime Date { get; set; }
        public string? Description { get; set; }
        public required string Category { get; set; }
        public required string City { get; set; }
        public required string Venue { get; set; }
        public required bool IsCancelled { get; set; }
        public required AttendeeDto Host { get; set; }
        public List<AttendeeDto> Attendees { get; set; } = new();
    }
}
