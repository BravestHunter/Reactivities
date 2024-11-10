using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Reactivities.Application.Mediator.Photos;
using Reactivities.Domain.Photos.Commands;

namespace Reactivities.Api.Controllers
{
    public class PhotosController : BaseApiController
    {
        public PhotosController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery, BindRequired] string fileName, IFormFile file)
        {
            using var stream = file.OpenReadStream();
            return HandleResult(await Mediator.Send(new AddPhotoCommand() { Stream = stream, FileName = fileName }));
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
