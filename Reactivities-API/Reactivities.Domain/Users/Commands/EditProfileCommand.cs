using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Users.Dtos;

namespace Reactivities.Domain.Users.Commands
{
    public class EditProfileCommand : IRequest<Result<ProfileDto>>
    {
        public required EditProfileDto Profile { get; set; }
    }
}
