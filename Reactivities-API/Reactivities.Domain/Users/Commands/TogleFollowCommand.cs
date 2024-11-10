using MediatR;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Users.Commands
{
    public class TogleFollowCommand : IRequest<Result>
    {
        public required string TargetUsername { get; set; }
    }
}
