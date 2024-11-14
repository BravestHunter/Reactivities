using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Domain.Users.Commands;
using Reactivities.Domain.Users.Queries;

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
            switch (predicate)
            {
                case "followers":
                    return HandleResult(await Mediator.Send(new GetFollowersListQuery() { Username = username }));

                case "following":
                    return HandleResult(await Mediator.Send(new GetFollowingListQuery() { Username = username }));
            }

            return NotFound("Predicate is unknown");
        }
    }
}
