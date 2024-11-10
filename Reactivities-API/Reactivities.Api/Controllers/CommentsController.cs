using MediatR;
using Microsoft.AspNetCore.Mvc;
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
    }
}
