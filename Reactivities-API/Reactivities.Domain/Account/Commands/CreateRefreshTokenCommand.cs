using MediatR;
using Reactivities.Domain.Account.Models;
using Reactivities.Domain.Core;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Account.Commands
{
    public class CreateRefreshTokenCommand : IRequest<Result<RefreshToken>>
    {
        public required AppUser User;
    }
}
