using System.Security.Cryptography;
using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Account.Models;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Account.Commands.Handlers
{
    internal sealed class CreateRefreshTokenHandler : IRequestHandler<CreateRefreshTokenCommand, Result<RefreshToken>>
    {
        private readonly IUserRepository _userRepository;
        private readonly RandomNumberGenerator _rng;
        private readonly ILogger _logger;

        public CreateRefreshTokenHandler(IUserRepository userRepository, RandomNumberGenerator rng, ILogger<CreateRefreshTokenHandler> logger)
        {
            _userRepository = userRepository;
            _rng = rng;
            _logger = logger;
        }

        public async Task<Result<RefreshToken>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByUsername(request.Username);
                if (user == null)
                {
                    return Result<RefreshToken>.Failure(new NotFoundException("Failed to find user with given RefreshToken"));
                }

                string token = GenerateRefreshToken();
                var resfreshToken = new RefreshToken()
                {
                    Token = token,
                    Expires = DateTime.UtcNow + request.RefreshTokenLifetime,
                    User = user
                };
                user.RefreshTokens.Add(resfreshToken);

                await _userRepository.Update(user);

                _logger.LogInformation("Created refreshToken for user {UserName}", user.UserName);

                return Result<RefreshToken>.Success(resfreshToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create refresh token");
                return Result<RefreshToken>.Failure(ex);
            }
        }

        private string GenerateRefreshToken()
        {
            var bytes = new byte[32];
            _rng.GetBytes(bytes);

            return Convert.ToBase64String(bytes);
        }
    }
}
