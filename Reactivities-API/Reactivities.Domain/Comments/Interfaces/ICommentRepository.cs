using Reactivities.Domain.Comments.Dtos;

namespace Reactivities.Domain.Comments.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<CommentDto>> GetDtoList(long activityId);
    }
}
