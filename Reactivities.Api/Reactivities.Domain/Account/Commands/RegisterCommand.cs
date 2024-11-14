using MediatR;
using Reactivities.Domain.Account.Dtos;
using Reactivities.Domain.Core;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Account.Commands
{
    public class RegisterCommand : IRequest<Result<CurrentUserDto>>
    {
        public required AppUser User { get; set; }
        public required string Password { get; set; }
    }
}
