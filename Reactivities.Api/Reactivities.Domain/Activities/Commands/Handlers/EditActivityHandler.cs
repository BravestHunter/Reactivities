using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;

namespace Reactivities.Domain.Activities.Commands.Handlers
{
    internal sealed class EditActivityHandler : IRequestHandler<EditActivityCommand, Result<ActivityDto>>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EditActivityHandler(IActivityRepository activityRepository, IMapper mapper, ILogger<EditActivityHandler> logger)
        {
            _activityRepository = activityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ActivityDto>> Handle(EditActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingActivity = await _activityRepository.GetByIdWithAttendees(request.Activity.Id);
                if (existingActivity == null)
                {
                    return Result<ActivityDto>.Failure(new NotFoundException("Failed to find activity"));
                }

                _mapper.Map(request.Activity, existingActivity);

                _logger.LogInformation("Edited activity {Id}", existingActivity.Id);

                var activityDto = await _activityRepository.Update(existingActivity);
                return Result<ActivityDto>.Success(activityDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to edit activity");
                return Result<ActivityDto>.Failure(ex);
            }
        }
    }
}
