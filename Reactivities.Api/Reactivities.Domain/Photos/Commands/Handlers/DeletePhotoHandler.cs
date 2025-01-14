﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Photos.Interfaces;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Photos.Commands.Handlers
{
    internal sealed class DeletePhotoHandler : IRequestHandler<DeletePhotoCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPhotoStorage _photoStorage;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public DeletePhotoHandler(
            IUserRepository userRepository,
            IPhotoStorage photoStorage,
            IUserAccessor userAccessor,
            IMapper mapper,
            ILogger<DeletePhotoHandler> logger
            )
        {
            _userRepository = userRepository;
            _photoStorage = photoStorage;
            _userAccessor = userAccessor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result> Handle(DeletePhotoCommand request, CancellationToken cancellationToken)
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

                if (currentUser.ProfilePhoto == photo)
                {
                    currentUser.ProfilePhoto = null;
                }
                currentUser.Photos.Remove(photo);

                await _userRepository.Update(currentUser);

                await _photoStorage.Delete(photo.StorageId);

                _logger.LogInformation("Deleted photo {Id}", photo.Id);

                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete photo");
                return Result.Failure(ex);
            }
        }
    }
}
