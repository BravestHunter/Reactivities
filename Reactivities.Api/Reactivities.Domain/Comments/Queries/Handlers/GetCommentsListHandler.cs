using MediatR;
using Reactivities.Domain.Comments.Dtos;
using Reactivities.Domain.Comments.Interfaces;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Comments.Queries.Handlers
{
    internal class GetCommentsListHandler : IRequestHandler<GetCommentsListQuery, Result<List<CommentDto>>>
    {
        private readonly ICommentRepository _commentRepository;

        public GetCommentsListHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<Result<List<CommentDto>>> Handle(GetCommentsListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var comments = await _commentRepository.GetDtoList(request.ActivityId);
                return Result<List<CommentDto>>.Success(comments);
            }
            catch (Exception ex)
            {
                return Result<List<CommentDto>>.Failure(ex);
            }
        }
    }
}
