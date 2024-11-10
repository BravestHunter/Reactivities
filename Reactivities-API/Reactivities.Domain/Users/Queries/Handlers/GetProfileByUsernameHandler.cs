using AutoMapper;
using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Users.Queries.Handlers
{
    internal class GetProfileByUsernameHandler : IRequestHandler<GetProfileByUsernameQuery, Result<ProfileDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public GetProfileByUsernameHandler(IUserRepository userRepository, IUserAccessor userAccessor, IMapper mapper)
        {
            _userRepository = userRepository;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result<ProfileDto>> Handle(GetProfileByUsernameQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();
                var profile = await _userRepository.GetProfileDto(request.Username, currentUsername);
                if (profile == null)
                {
                    return Result<ProfileDto>.Failure(new NotFoundException("Failed to find user profile"));
                }

                return Result<ProfileDto>.Success(profile);
            }
            catch (Exception ex)
            {
                return Result<ProfileDto>.Failure(ex);
            }
        }
    }
}
