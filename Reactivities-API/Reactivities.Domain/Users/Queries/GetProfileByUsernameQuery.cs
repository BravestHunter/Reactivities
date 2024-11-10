using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Users.Dtos;

namespace Reactivities.Domain.Users.Queries
{
    public class GetProfileByUsernameQuery : IRequest<Result<ProfileDto>>
    {
        public required string Username { get; set; }
    }
}
