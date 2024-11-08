using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Interfaces;
using Reactivities.Domain.Models;
using Reactivities.Persistence;

namespace Reactivities.Application.Followers
{
    public class FollowToggle
    {
        public class Command : IRequest<Result>
        {
            public string TargetUsername { get; set; }
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
                    var username = _userAccessor.GetUsername();
                    var observer = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == username);

                    var target = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == request.TargetUsername);
                    if (target == null)
                    {
                        return Result.Failure("Failed to find target user");
                    }

                    var following = await _dataContext.UserFollowings.FindAsync(observer.Id, target.Id);
                    if (following == null)
                    {
                        following = new UserFollowing
                        {
                            Observer = observer,
                            Target = target
                        };
                        _dataContext.UserFollowings.Add(following);
                    }
                    else
                    {
                        _dataContext.UserFollowings.Remove(following);
                    }

                    var result = await _dataContext.SaveChangesAsync() > 0;

                    if (!result)
                    {
                        return Result.Failure("Failed to update following");
                    }

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result.Failure("Failed to update following", ex);
                }
            }
        }
    }
}
