using MediatR;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;

namespace Reactivities.Domain.Activities.Commands.Handlers
{
    internal class DeleteActivityHandler : IRequestHandler<DeleteActivityCommand, Result>
    {
        private readonly IActivityRepository _activityRepository;

        public DeleteActivityHandler(IActivityRepository activityRepository)
        {
            _activityRepository = activityRepository;
        }

        public async Task<Result> Handle(DeleteActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingActivity = await _activityRepository.GetById(request.Id);
                if (existingActivity == null)
                {
                    return Result.Failure(new NotFoundException("Failed to find activity"));
                }

                await _activityRepository.Delete(existingActivity);
                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }
    }
}
