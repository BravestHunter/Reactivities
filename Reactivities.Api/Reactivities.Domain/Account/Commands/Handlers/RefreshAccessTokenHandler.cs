using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Account.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Account.Commands.Handlers
{
    internal sealed class RefreshAccessTokenHandler : IRequestHandler<RefreshAccessTokenCommand, Result<string>>
    {
        private readonly ITokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public RefreshAccessTokenHandler(ITokenService tokenService, IUserRepository userRepository, ILogger<RefreshAccessTokenHandler> logger)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(RefreshAccessTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByRefreshToken(request.RefreshToken);
                if (user == null)
                {
                    return Result<string>.Failure(new NotFoundException("Failed to find user with given RefreshToken"));
                }

                var oldToken = user.RefreshTokens.SingleOrDefault(t => t.Token == request.RefreshToken);
                if (oldToken != null && !oldToken.IsActive)
                {
                    return Result<string>.Failure(new BadRequestException("Given RefreshToken is invalid"));
                }

                var accessToken = _tokenService.GenerateAccessToken(user);

                _logger.LogInformation("Refreshed AccessToken for user {UserName}", user.UserName);

                return Result<string>.Success(accessToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh AccessToken");
                return Result<string>.Failure(ex);
            }
        }
    }
}
