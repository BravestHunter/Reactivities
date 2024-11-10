using AutoMapper;
using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Users.Commands.Handlers
{
    internal class EditProfileHandler : IRequestHandler<EditProfileCommand, Result<ProfileDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public EditProfileHandler(IUserRepository userRepository, IUserAccessor userAccessor, IMapper mapper)
        {
            _userRepository = userRepository;
            _userAccessor = userAccessor;
            _mapper = mapper;
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

                return Result<ProfileDto>.Success(profileDto);
            }
            catch (Exception ex)
            {
                return Result<ProfileDto>.Failure(ex);
            }
        }
    }
}
