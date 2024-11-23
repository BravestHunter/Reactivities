using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Activities.Commands.Handlers
{
    internal sealed class CreateActivityHandler : IRequestHandler<CreateActivityCommand, Result<ActivityDto>>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CreateActivityHandler(
            IActivityRepository activityRepository,
            IUserRepository userRepository,
            IUserAccessor userAccessor,
            IMapper mapper,
            ILogger<CreateActivityHandler> logger)
        {
            _activityRepository = activityRepository;
            _userRepository = userRepository;
            _userAccessor = userAccessor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ActivityDto>> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();
                var currentUser = await _userRepository.GetByUsername(currentUsername);
                if (currentUser == null)
                {
                    return Result<ActivityDto>.Failure(new NotFoundException("Failed to find current user"));
                }

                var activity = _mapper.Map<Activity>(request.Activity);
                activity.Host = currentUser;

                _logger.LogInformation("Created actyivity {Id}", activity.Id);

                var activityDto = await _activityRepository.Add(activity);
                return Result<ActivityDto>.Success(activityDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create activity");
                return Result<ActivityDto>.Failure(ex);
            }
        }
    }
}
