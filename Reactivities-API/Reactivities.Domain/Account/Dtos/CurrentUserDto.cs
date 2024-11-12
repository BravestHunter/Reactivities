namespace Reactivities.Domain.Account.Dtos
{
    public class CurrentUserDto
    {
        public required string Username { get; set; }
        public required string DisplayName { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
