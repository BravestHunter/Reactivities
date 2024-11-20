using AutoMapper;
using MediatR;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;

namespace Reactivities.Domain.Activities.Commands.Handlers
{
    internal class EditActivityHandler : IRequestHandler<EditActivityCommand, Result<ActivityDto>>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IMapper _mapper;

        public EditActivityHandler(IActivityRepository activityRepository, IMapper mapper)
        {
            _activityRepository = activityRepository;
            _mapper = mapper;
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

                var activityDto = await _activityRepository.Update(existingActivity);
                return Result<ActivityDto>.Success(activityDto);
            }
            catch (Exception ex)
            {
                return Result<ActivityDto>.Failure(ex);
            }
        }
    }
}
