using MediatR;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Account.Commands
{
    public class RefreshAccessTokenCommand : IRequest<Result<string>>
    {
        public required string RefreshToken { get; set; }
    }
}
