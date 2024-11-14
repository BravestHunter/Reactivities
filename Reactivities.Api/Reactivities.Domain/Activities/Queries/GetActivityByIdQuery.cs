using MediatR;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Activities.Queries
{
    public class GetActivityByIdQuery : IRequest<Result<ActivityDto>>
    {
        public required long Id { get; set; }
    }
}
