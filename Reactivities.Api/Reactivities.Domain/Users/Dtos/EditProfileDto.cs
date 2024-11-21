namespace Reactivities.Domain.Users.Dtos
{
    public class EditProfileDto
    {
        public required string DisplayName { get; set; }
        public string? Bio { get; set; }
    }
}
