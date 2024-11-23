using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;

namespace Reactivities.Domain.Activities.Commands.Handlers
{
    internal sealed class DeleteActivityHandler : IRequestHandler<DeleteActivityCommand, Result>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly ILogger _logger;

        public DeleteActivityHandler(IActivityRepository activityRepository, ILogger<DeleteActivityHandler> logger)
        {
            _activityRepository = activityRepository;
            _logger = logger;
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

                _logger.LogInformation("Deleted activity {Id}", existingActivity.Id);

                await _activityRepository.Delete(existingActivity);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete activity");
                return Result.Failure(ex);
            }
        }
    }
}
