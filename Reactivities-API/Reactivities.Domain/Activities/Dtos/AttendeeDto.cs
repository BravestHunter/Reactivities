namespace Reactivities.Domain.Activities.Dtos
{
    public class AttendeeDto
    {
        public required string Username { get; set; }
        public required string DisplayName { get; set; }
        public string? Bio { get; set; }
        public required string ProfilePhotoUrl { get; set; }
        public required bool Following { get; set; }
        public required int FollowersCount { get; set; }
        public required int FollowingCount { get; set; }
    }
}
