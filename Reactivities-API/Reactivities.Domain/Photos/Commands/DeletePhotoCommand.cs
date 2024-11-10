using MediatR;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Photos.Commands
{
    public class DeletePhotoCommand : IRequest<Result>
    {
        public required long Id { get; set; }
    }
}
