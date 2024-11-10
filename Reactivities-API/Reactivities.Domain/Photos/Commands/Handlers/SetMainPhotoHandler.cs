using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Photos.Commands.Handlers
{
    internal class SetMainPhotoHandler : IRequestHandler<SetMainPhotoCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAccessor _userAccessor;

        public SetMainPhotoHandler(IUserRepository userRepository, IUserAccessor userAccessor)
        {
            _userRepository = userRepository;
            _userAccessor = userAccessor;
        }

        public async Task<Result> Handle(SetMainPhotoCommand request, CancellationToken cancellationToken)
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
                    return Result.Failure("Failed to find photo");
                }

                var mainPhoto = currentUser.Photos.FirstOrDefault(p => p.IsMain);
                if (mainPhoto != null)
                {
                    mainPhoto.IsMain = false;
                }

                photo.IsMain = true;

                await _userRepository.Update(currentUser);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }
    }
}
