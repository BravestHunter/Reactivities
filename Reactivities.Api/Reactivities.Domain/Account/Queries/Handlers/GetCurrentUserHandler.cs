using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Account.Dtos;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Account.Queries.Handlers
{
    internal sealed class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, Result<CurrentUserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly ILogger _logger;

        public GetCurrentUserHandler(IUserRepository userRepository, IUserAccessor userAccessor, ILogger<GetCurrentUserHandler> logger)
        {
            _userRepository = userRepository;
            _userAccessor = userAccessor;
            _logger = logger;
        }

        public async Task<Result<CurrentUserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();
                var currentUserDto = await _userRepository.GetCurrentUserDto(currentUsername);
                if (currentUserDto == null)
                {
                    return Result<CurrentUserDto>.Failure(new NotFoundException("Failed to find current user"));
                }

                _logger.LogInformation("Found current user {CurrentUsername}", currentUsername);

                return Result<CurrentUserDto>.Success(currentUserDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get current user");
                return Result<CurrentUserDto>.Failure(ex);
            }
        }
    }
}
