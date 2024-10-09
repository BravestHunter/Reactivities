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
        public class Command : IRequest<Result<Unit>>
        {
            public string TargetUsername { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
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
                var username = _userAccessor.GetUsername();
                var observer = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == username);

                var target = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == request.TargetUsername);
                if (target == null)
                {
                    return null;
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
                    return Result<Unit>.Failure("Failed to update following");
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
