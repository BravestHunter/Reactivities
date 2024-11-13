using System.Security.Cryptography;
using MediatR;
using Reactivities.Domain.Account.Models;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Account.Commands.Handlers
{
    internal class CreateRefreshTokenHandler : IRequestHandler<CreateRefreshTokenCommand, Result<RefreshToken>>
    {
        private readonly IUserRepository _userRepository;
        private readonly RandomNumberGenerator _rng;

        public CreateRefreshTokenHandler(IUserRepository userRepository, RandomNumberGenerator rng)
        {
            _userRepository = userRepository;
            _rng = rng;
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

                return Result<RefreshToken>.Success(resfreshToken);
            }
            catch (Exception ex)
            {
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
