using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Account.Commands
{
    public class LoginCommand : IRequest<Result<AppUser>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
