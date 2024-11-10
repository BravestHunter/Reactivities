using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Mediator.Profiles;
using Reactivities.Domain.Users.Commands;
using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Queries;

namespace Reactivities.Api.Controllers
{
    public class ProfilesController : BaseApiController
    {
        public ProfilesController(IMediator mediator) : base(mediator) { }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new GetProfileByUsernameQuery() { Username = username }));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(EditProfileDto profile)
        {
            return HandleResult(await Mediator.Send(new EditProfileCommand() { Profile = profile }));
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
