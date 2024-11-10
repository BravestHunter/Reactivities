using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Account.Commands
{
    public class RegisterCommand : IRequest<Result<AppUser>>
    {
        public required AppUser User { get; set; }
        public required string Password { get; set; }
    }
}
