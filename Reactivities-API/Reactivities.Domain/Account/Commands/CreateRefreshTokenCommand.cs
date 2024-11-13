using MediatR;
using Reactivities.Domain.Account.Models;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Account.Commands
{
    public class CreateRefreshTokenCommand : IRequest<Result<RefreshToken>>
    {
        public required string Username { get; set; }
        public required TimeSpan RefreshTokenLifetime { get; set; }
    }
}
