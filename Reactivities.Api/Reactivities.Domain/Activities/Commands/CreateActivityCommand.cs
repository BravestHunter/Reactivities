using MediatR;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Activities.Commands
{
    public class CreateActivityCommand : IRequest<Result<ActivityDto>>
    {
        public required CreateActivityDto Activity { get; set; }
    }
}
