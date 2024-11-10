using AutoMapper;
using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Photos.Interfaces;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Photos.Commands.Handlers
{
    internal class DeletePhotoHandler : IRequestHandler<DeletePhotoCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoStorage _photoStorage;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public DeletePhotoHandler(
            IUserRepository userRepository,
            IPhotoStorage photoStorage,
            IUserAccessor userAccessor,
            IMapper mapper
            )
        {
            _userRepository = userRepository;
            _photoStorage = photoStorage;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string currentUsername = _userAccessor.GetUsername();
                var user = await _userRepository.GetByUsernameWithPhotos(currentUsername);
                if (user == null)
                {
                    return Result.Failure(new NotFoundException("Failed to find current user"));
                }

                var photo = user.Photos.FirstOrDefault(p => p.Id == request.Id);
                if (photo == null)
                {
                    return Result.Failure(new NotFoundException("Failed to find photo"));
                }

                if (photo.IsMain)
                {
                    return Result.Failure(new BadRequestException("Main photo can't be deleted"));
                }

                await _photoStorage.Delete(photo.StorageId);

                user.Photos.Remove(photo);
                await _userRepository.Update(user);

                return Result.Success();
            }
            catch (Exception ex)
            {
                return Result.Failure(ex);
            }
        }
    }
}
