using MediatR;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Activities.Commands
{
    public class UpdateAttendanceCommand : IRequest<Result>
    {
        public required long Id { get; set; }
        public required bool Attend { get; set; }
    }
}
