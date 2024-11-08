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
        public class Command : IRequest<Result>
        {
            public long Id { get; set; }
        }

        internal class Handler : IRequestHandler<Command, Result>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var activity = await _dataContext.Activities
                        .Include(a => a.Attendees)
                        .ThenInclude(u => u.AppUser)
                        .FirstOrDefaultAsync(a => a.Id == request.Id);
                    if (activity == null)
                    {
                        return Result.Failure("Failed to find activity");
                    }

                    var username = _userAccessor.GetUsername();
                    var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
                    if (user == null)
                    {
                        return Result.Failure("Failed to find user");
                    }

                    var hostUsername = activity.Host.UserName;

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
                            Activity = activity
                        };

                        activity.Attendees.Add(attendance);
                    }

                    var result = await _dataContext.SaveChangesAsync() > 0;

                    if (!result)
                    {
                        return Result.Failure("Failed to update attendance");
                    }

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result.Failure("Failed to update attendance", ex);
                }
            }
        }
    }
}
