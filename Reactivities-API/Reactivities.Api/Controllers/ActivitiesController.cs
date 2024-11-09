using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Api.Dto;
using Reactivities.Application.Mediator.Activities;
using Reactivities.Domain.Activities.Filters;
using Reactivities.Domain.Activities.Models;
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
        public async Task<IActionResult> CreateActivity(Activity activity)
        {
            return HandleResult(await Mediator.Send(new Create.Command() { Activity = activity }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(long id, Activity activity)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command() { Activity = activity }));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(long id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command() { Id = id }));
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(long id)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command() { Id = id }));
        }
    }
}
