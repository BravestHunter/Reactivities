using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Api.Models;
using Reactivities.Domain.Activities.Filters;
using Reactivities.Domain.Activities.Queries;
using Reactivities.Domain.Core;
using Reactivities.Domain.Users.Commands;
using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Queries;

namespace Reactivities.Api.Controllers
{
    public class ProfilesController(IMediator mediator) : BaseApiController(mediator)
    {
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
        public async Task<IActionResult> GetUserActivities([FromQuery] GetActivitiesRequest request, string username)
        {
            return HandleResult(await Mediator.Send(new GetUserActivitiesListQuery()
            {
                PagingParams = new PagingParams(request.PageNumber, request.PageSize),
                Filters = new UserActivityListFilters()
                {
                    FromDate = request.FromDate,
                    ToDate = request.ToDate,
                    Relationship = request.Relationship,
                    TargetUsername = username
                }
            }));
        }
    }
}
