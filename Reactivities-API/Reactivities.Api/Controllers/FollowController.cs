using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Followers;
using Reactivities.Domain.Users.Commands;

namespace Reactivities.Api.Controllers
{
    public class FollowController : BaseApiController
    {
        public FollowController(IMediator mediator) : base(mediator) { }

        [HttpPost("{username}")]
        public async Task<IActionResult> Follow(string username)
        {
            return HandleResult(await Mediator.Send(new TogleFollowCommand() { TargetUsername = username }));
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetFollowings(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new List.Query() { Username = username, Predicate = predicate }));
        }
    }
}
