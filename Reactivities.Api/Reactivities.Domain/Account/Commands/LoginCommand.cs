using MediatR;
using Reactivities.Domain.Account.Dtos;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Account.Commands
{
    public class LoginCommand : IRequest<Result<CurrentUserDto>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
