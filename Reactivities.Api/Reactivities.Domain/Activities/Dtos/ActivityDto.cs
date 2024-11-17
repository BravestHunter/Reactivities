using Reactivities.Domain.Users.Dtos;

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
        public required ProfileShortDto Host { get; set; }
        public List<ProfileShortDto> Attendees { get; set; } = new();
    }
}
