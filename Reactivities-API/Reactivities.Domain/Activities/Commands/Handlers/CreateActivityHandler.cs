using AutoMapper;
using MediatR;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Activities.Commands.Handlers
{
    internal class CreateActivityHandler : IRequestHandler<CreateActivityCommand, Result<ActivityDto>>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public CreateActivityHandler(IActivityRepository activityRepository, IUserRepository userRepository, IUserAccessor userAccessor, IMapper mapper)
        {
            _activityRepository = activityRepository;
            _userRepository = userRepository;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result<ActivityDto>> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();
                var currentUser = await _userRepository.GetByUsername(currentUsername);
                if (currentUser == null)
                {
                    return Result<ActivityDto>.Failure("Failed to find current user");
                }

                var activity = _mapper.Map<Activity>(request.Activity);
                activity.Host = currentUser;

                var activityDto = await _activityRepository.Add(activity);
                return Result<ActivityDto>.Success(activityDto);
            }
            catch (Exception ex)
            {
                return Result<ActivityDto>.Failure(ex);
            }
        }
    }
}
