using AutoMapper;
using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Photos.Dtos;
using Reactivities.Domain.Photos.Interfaces;
using Reactivities.Domain.Photos.Models;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Photos.Commands.Handlers
{
    internal class AddPhotoHandler : IRequestHandler<AddPhotoCommand, Result<PhotoDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoStorage _photoStorage;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public AddPhotoHandler(
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

        public async Task<Result<PhotoDto>> Handle(AddPhotoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                string currentUsername = _userAccessor.GetUsername();
                var currentUser = await _userRepository.GetByUsernameWithPhotos(currentUsername);
                if (currentUser == null)
                {
                    return Result<PhotoDto>.Failure(new NotFoundException("Failed to find current user"));
                }

                var uploadResult = await _photoStorage.Add(request.Stream, request.FileName);
                var photo = new Photo
                {
                    Url = uploadResult.Url,
                    StorageId = uploadResult.StorageId
                };

                if (!currentUser.Photos.Any())
                {
                    photo.IsMain = true;
                }

                currentUser.Photos.Add(photo);
                await _userRepository.Update(currentUser);

                var photoDto = _mapper.Map<PhotoDto>(photo);
                return Result<PhotoDto>.Success(photoDto);
            }
            catch (Exception ex)
            {
                return Result<PhotoDto>.Failure(ex);
            }
        }
    }
}
