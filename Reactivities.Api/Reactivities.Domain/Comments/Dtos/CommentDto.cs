namespace Reactivities.Domain.Comments.Dtos
{
    public class CommentDto
    {
        public required long Id { get; set; }
        public required string Body { get; set; }
        public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public required string Username { get; set; }
        public required string DisplayName { get; set; }
        public string? ProfilePhotoUrl { get; set; }
    }
}
