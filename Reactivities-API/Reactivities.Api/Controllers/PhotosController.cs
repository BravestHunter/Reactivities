using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Photos;

namespace Reactivities.Api.Controllers
{
    public class PhotosController : BaseApiController
    {
        public PhotosController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] Add.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command() { Id = id }));
        }

        [HttpPost("{id}/setmain")]
        public async Task<IActionResult> SetMain(long id)
        {
            return HandleResult(await Mediator.Send(new SetMain.Command() { Id = id }));
        }
    }
}
