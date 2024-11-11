namespace Reactivities.Application.Dtos
{
    public class UserResponseDto
    {
        public required string Username { get; set; }
        public required string DisplayName { get; set; }
        public string? Image { get; set; }
        public required string AccessToken { get; set; }
    }
}
