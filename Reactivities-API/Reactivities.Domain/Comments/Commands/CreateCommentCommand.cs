using MediatR;
using Reactivities.Domain.Comments.Dtos;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Comments.Commands
{
    public class CreateCommentCommand : IRequest<Result<CommentDto>>
    {
        public required CreateCommentDto Comment { get; set; }
    }
}
