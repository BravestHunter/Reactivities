using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Api.Dto;
using Reactivities.Application.Mediator.Activities;
using Reactivities.Domain.Activities.Commands;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Filters;
using Reactivities.Domain.Activities.Queries;
using Reactivities.Domain.Core;

namespace Reactivities.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : BaseApiController
    {
        public ActivitiesController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<IActionResult> GetActivities([FromQuery] GetActivitiesRequestDto request)
        {
            return HandlePagedResult(await Mediator.Send(new GetActivityListQuery()
            {
                PagingParams = new PagingParams(request.PageNumber, request.PageSize),
                Filters = new ActivityListFilters()
                {
                    StartDate = request.StartDate,
                    Relationship = request.Relationship
                }
            }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(long id)
        {
            return HandleResult(await Mediator.Send(new GetActivityByIdQuery() { Id = id }));
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(CreateActivityDto activity)
        {
            return HandleResult(await Mediator.Send(new CreateActivityCommand() { Activity = activity }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(long id, EditActivityDto activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new EditActivityCommand() { Activity = activity }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(long id)
        {
            return HandleResult(await Mediator.Send(new DeleteActivityCommand() { Id = id }));
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(long id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command() { Id = id }));
        }
    }
}
