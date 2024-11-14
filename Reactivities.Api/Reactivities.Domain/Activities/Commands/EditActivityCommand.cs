using MediatR;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Activities.Commands
{
    public class EditActivityCommand : IRequest<Result<ActivityDto>>
    {
        public required EditActivityDto Activity { get; set; }
    }
}
