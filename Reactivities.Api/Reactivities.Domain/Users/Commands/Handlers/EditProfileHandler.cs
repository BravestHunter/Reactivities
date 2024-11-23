using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Users.Commands.Handlers
{
    internal sealed class EditProfileHandler : IRequestHandler<EditProfileCommand, Result<ProfileDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public EditProfileHandler(
            IUserRepository userRepository,
            IUserAccessor userAccessor,
            IMapper mapper,
            ILogger<EditProfileHandler> logger
            )
        {
            _userRepository = userRepository;
            _userAccessor = userAccessor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ProfileDto>> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();
                var user = await _userRepository.GetByUsername(currentUsername);
                if (user == null)
                {
                    return Result<ProfileDto>.Failure(new NotFoundException("Failed to find current user"));
                }

                _mapper.Map(request.Profile, user);

                await _userRepository.Update(user);

                var profileDto = await _userRepository.GetProfileDto(currentUsername, currentUsername);
                if (profileDto == null)
                {
                    return Result<ProfileDto>.Failure(new NotFoundException("Failed to find current user profile"));
                }

                _logger.LogInformation("Edited profile {UserName}", currentUsername);

                return Result<ProfileDto>.Success(profileDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to edit profile");
                return Result<ProfileDto>.Failure(ex);
            }
        }
    }
}
