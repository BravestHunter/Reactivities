using MediatR;
using Reactivities.Domain.Comments.Dtos;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Comments.Queries
{
    public class GetCommentsListQuery : IRequest<Result<List<CommentDto>>>
    {
        public required long ActivityId { get; set; }
    }
}
