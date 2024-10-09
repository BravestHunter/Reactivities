using MediatR;
using Reactivities.Application.Core;
using Reactivities.Persistence;

namespace Reactivities.Application.Activities
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public long Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var existingActivity = await _dataContext.Activities.FindAsync(request.Id);
                if (existingActivity == null)
                {
                    return null;
                }

                _dataContext.Remove(existingActivity);

                var result = await _dataContext.SaveChangesAsync() > 0;
                if (@result)
                {
                    return Result<Unit>.Failure("Failed to delete activity");
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
