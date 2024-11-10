using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Domain.Comments.Commands;
using Reactivities.Domain.Comments.Dtos;
using Reactivities.Domain.Comments.Queries;

namespace Reactivities.Api.Controllers
{
    public class CommentsController : BaseApiController
    {
        public CommentsController(IMediator mediator) : base(mediator) { }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetComments(long id)
        {
            return HandleResult(await Mediator.Send(new GetCommentsListQuery() { ActivityId = id }));
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> CreateComment(long id, [FromBody] string body)
        {
            return HandleResult(await Mediator.Send(new CreateCommentCommand()
            {
                Comment = new CreateCommentDto() { ActivityId = id, Body = body }
            }));
        }
    }
}
