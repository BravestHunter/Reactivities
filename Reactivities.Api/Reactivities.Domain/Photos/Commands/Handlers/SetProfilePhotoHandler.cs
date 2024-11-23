using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Photos.Commands.Handlers
{
    internal sealed class SetProfilePhotoHandler(IUserRepository userRepository, IUserAccessor userAccessor, ILogger<SetProfilePhotoHandler> logger) : IRequestHandler<SetProfilePhotoCommand, Result>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUserAccessor _userAccessor = userAccessor;
        private readonly ILogger _logger = logger;

        public async Task<Result> Handle(SetProfilePhotoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string currentUsername = _userAccessor.GetUsername();
                var currentUser = await _userRepository.GetByUsernameWithPhotos(currentUsername);
                if (currentUser == null)
                {
                    return Result.Failure(new NotFoundException("Failed to find current user"));
                }

                var photo = currentUser.Photos.FirstOrDefault(p => p.Id == request.Id);
                if (photo == null)
                {
                    return Result.Failure(new NotFoundException("Failed to find photo"));
                }

                currentUser.ProfilePhoto = photo;

                await _userRepository.Update(currentUser);

                _logger.LogInformation("Set profile photo {PhotoId} for user {CurrentUsername}", photo.Id, currentUsername);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to set profile photo");
                return Result.Failure(ex);
            }
        }
    }
}
