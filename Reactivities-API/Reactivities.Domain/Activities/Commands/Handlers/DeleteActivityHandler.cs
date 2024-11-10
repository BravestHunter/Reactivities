using MediatR;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Exceptions;

namespace Reactivities.Domain.Activities.Commands.Handlers
{
    internal class DeleteActivityHandler : IRequestHandler<DeleteActivityCommand, Result>
    {
        private readonly IActivityRepository _repository;

        public DeleteActivityHandler(IActivityRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingActivity = await _repository.GetById(request.Id);
                if (existingActivity == null)
                {
                    return Result.Failure(new NotFoundException("Failed to find activity"));
                }

                await _repository.Delete(existingActivity);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }
    }
}
