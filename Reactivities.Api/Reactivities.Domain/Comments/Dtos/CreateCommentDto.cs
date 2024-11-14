namespace Reactivities.Domain.Comments.Dtos
{
    public class CreateCommentDto
    {
        public required string Body { get; set; }
        public required long ActivityId { get; set; }
    }
}
