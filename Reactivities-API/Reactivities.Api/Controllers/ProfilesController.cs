using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Profiles;

namespace Reactivities.Api.Controllers
{
    public class ProfilesController : BaseApiController
    {
        public ProfilesController(IMediator mediator) : base(mediator) { }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new Details.Query() { Username = username }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(Edit.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpGet("{username}/activities")]
        public async Task<IActionResult> GetUserActivities(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(
                new ListActivities.Query() { Username = username, Predicate = predicate }
                ));
        }
    }
}
