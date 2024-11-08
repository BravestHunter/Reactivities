using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Mediator.Activities;
using Reactivities.Domain.Models;

namespace Reactivities.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ActivitiesController : BaseApiController
    {
        public ActivitiesController(IMediator mediator) : base(mediator) { }

        [HttpGet]
        public async Task<IActionResult> GetActivities([FromQuery] ActivityParams activityParams)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query() { Params = activityParams }));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetActivity(long id)
        {
            return HandleResult(await Mediator.Send(new Details.Query() { Id = id }));
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
