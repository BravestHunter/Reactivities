using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Photos.Dtos;
using Reactivities.Domain.Photos.Interfaces;
using Reactivities.Domain.Photos.Models;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Photos.Commands.Handlers
{
    internal sealed class AddPhotoHandler : IRequestHandler<AddPhotoCommand, Result<PhotoDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoStorage _photoStorage;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public AddPhotoHandler(
            IUserRepository userRepository,
            IPhotoStorage photoStorage,
            IUserAccessor userAccessor,
            IMapper mapper,
            ILogger<AddPhotoHandler> logger
            )
        {
            _userRepository = userRepository;
            _photoStorage = photoStorage;
            _userAccessor = userAccessor;
            _mapper = mapper;
            _logger = logger;
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

                var uploadResult = await _photoStorage.Add(request.Stream);
                var photo = new Photo
                {
                    Url = uploadResult.Url,
                    StorageId = uploadResult.StorageId
                };

                currentUser.Photos.Add(photo);
                await _userRepository.Update(currentUser);

                _logger.LogInformation("Added photo {Id}", photo.Id);

                var photoDto = _mapper.Map<PhotoDto>(photo);
                return Result<PhotoDto>.Success(photoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add photo");
                return Result<PhotoDto>.Failure(ex);
            }
        }
    }
}
