using MediatR;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Photos.Commands
{
    public class SetProfilePhotoCommand : IRequest<Result>
    {
        public required long Id { get; set; }
    }
}
