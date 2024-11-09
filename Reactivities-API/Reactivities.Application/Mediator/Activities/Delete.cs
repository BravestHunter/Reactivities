using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Persistence;

namespace Reactivities.Application.Mediator.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result>
        {
            public long Id { get; set; }
        }

        internal class Handler : IRequestHandler<Command, Result>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var existingActivity = await _dataContext.Activities.FindAsync(request.Id);
                    if (existingActivity == null)
                    {
                        return Result.Failure("Failed to find activity");
                    }

                    _dataContext.Remove(existingActivity);

                    var result = await _dataContext.SaveChangesAsync() > 0;
                    if (@result)
                    {
                        return Result.Failure("Failed to delete activity");
                    }

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result.Failure("Failed to delete activity", ex);
                }
            }
        }
    }
}
