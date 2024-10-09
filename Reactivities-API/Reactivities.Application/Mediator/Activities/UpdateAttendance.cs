using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Interfaces;
using Reactivities.Domain.Models;
using Reactivities.Persistence;

namespace Reactivities.Application.Mediator.Activities
{
    public class UpdateAttendance
    {
        public class Command : IRequest<Result<Unit>>
        {
            public long Id { get; set; }
        }

        internal class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
            }

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                var activity = await _dataContext.Activities
                    .Include(a => a.Attendees)
                    .ThenInclude(u => u.AppUser)
                    .FirstOrDefaultAsync(a => a.Id == request.Id);
                if (activity == null)
                {
                    return null;
                }

                var username = _userAccessor.GetUsername();
                var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    return null;
                }

                var hostUsername = activity.Attendees.FirstOrDefault(u => u.IsHost)?.AppUser?.UserName;

                var attendance = activity.Attendees.FirstOrDefault(u => u.AppUser.UserName == user.UserName);

                if (attendance != null)
                {
                    if (hostUsername == user.UserName)
                    {
                        activity.IsCancelled = !activity.IsCancelled;
                    }
                    else
                    {
                        activity.Attendees.Remove(attendance);
                    }
                }
                else
                {
                    attendance = new ActivityAttendee
                    {
                        AppUser = user,
                        Activity = activity,
                        IsHost = false
                    };

                    activity.Attendees.Add(attendance);
                }

                var result = await _dataContext.SaveChangesAsync() > 0;

                if (!result)
                {
                    return Result<Unit>.Failure("Failed to update attendance");
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
