using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Photos.Dtos;

namespace Reactivities.Domain.Photos.Commands
{
    public class AddPhotoCommand : IRequest<Result<PhotoDto>>
    {
        public required Stream Stream { get; set; }
    }
}
