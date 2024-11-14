using MediatR;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Activities.Commands
{
    public class DeleteActivityCommand : IRequest<Result>
    {
        public required long Id { get; set; }
    }
}
