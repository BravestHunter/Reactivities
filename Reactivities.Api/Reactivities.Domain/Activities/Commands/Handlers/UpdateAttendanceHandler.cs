using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Activities.Commands.Handlers
{
    internal sealed class UpdateAttendanceHandler : IRequestHandler<UpdateAttendanceCommand, Result>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UpdateAttendanceHandler(
            IActivityRepository activityRepository,
            IUserRepository userRepository,
            IUserAccessor userAccessor,
            IMapper mapper,
            ILogger<UpdateAttendanceHandler> logger)
        {
            _activityRepository = activityRepository;
            _userRepository = userRepository;
            _userAccessor = userAccessor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result> Handle(UpdateAttendanceCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var activity = await _activityRepository.GetByIdWithAttendees(request.Id);
                if (activity == null)
                {
                    return Result.Failure(new NotFoundException("Failed to find activity"));
                }

                var currentUsername = _userAccessor.GetUsername();
                if (activity.Host.UserName == currentUsername)
                {
                    return Result.Failure(new BadRequestException("Can't set attendance for host"));
                }

                var attendance = activity.Attendees.FirstOrDefault(a => a.User.UserName == currentUsername);
                if (!(request.Attend ^ attendance != null))
                {
                    // No changes needed
                    return Result.Success();
                }

                var currentUser = await _userRepository.GetByUsername(currentUsername);
                if (currentUser == null)
                {
                    return Result.Failure(new NotFoundException("Failed to find current user"));
                }

                if (request.Attend)
                {
                    attendance = new ActivityAttendee
                    {
                        User = currentUser,
                        Activity = activity
                    };

                    activity.Attendees.Add(attendance);
                }
                else if (attendance != null)
                {
                    activity.Attendees.Remove(attendance);
                }
                await _activityRepository.Update(activity);

                _logger.LogInformation("Updated activity {ActivityId} attendace for user {CurrentUsername} to {Attend}", activity.Id, currentUsername, request.Attend);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update activity attendance");
                return Result.Failure(ex);
            }
        }
    }
}
