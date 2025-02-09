using MediatR;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Domain.Photos.Commands;

namespace Reactivities.Api.Controllers
{
    public class PhotosController : BaseApiController
    {
        public PhotosController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        public async Task<IActionResult> Add(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            return HandleResult(await Mediator.Send(new AddPhotoCommand() { Stream = stream }));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            return HandleResult(await Mediator.Send(new DeletePhotoCommand() { Id = id }));
        }

        [HttpPost("{id}/setProfile")]
        public async Task<IActionResult> SetProfile(long id)
        {
            return HandleResult(await Mediator.Send(new SetProfilePhotoCommand() { Id = id }));
        }
    }
}
