using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Interfaces;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Users.Commands.Handlers
{
    internal class TogleFollowHandler : IRequestHandler<TogleFollowCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFollowingRepository _followingRepository;
        private readonly IUserAccessor _userAccessor;

        public TogleFollowHandler(IUserRepository userRepository, IUserFollowingRepository followingRepository, IUserAccessor userAccessor)
        {
            _userRepository = userRepository;
            _followingRepository = followingRepository;
            _userAccessor = userAccessor;
        }

        public async Task<Result> Handle(TogleFollowCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();
                var currentUser = await _userRepository.GetByUsername(currentUsername);
                if (currentUser == null)
                {
                    return Result.Failure(new NotFoundException("Failed to find current user"));
                }

                var target = await _userRepository.GetByUsername(request.TargetUsername);
                if (target == null)
                {
                    return Result.Failure(new NotFoundException("Failed to find target user"));
                }

                var following = await _followingRepository.GetByIds(currentUser.Id, target.Id);
                if (following == null)
                {
                    following = new UserFollowing
                    {
                        Observer = currentUser,
                        Target = target
                    };
                    await _followingRepository.Add(following);
                }
                else
                {
                    await _followingRepository.Delete(following);
                }

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }
    }
}
