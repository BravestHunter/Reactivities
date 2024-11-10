using MediatR;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Photos.Commands
{
    public class SetMainPhotoCommand : IRequest<Result>
    {
        public required long Id { get; set; }
    }
}
