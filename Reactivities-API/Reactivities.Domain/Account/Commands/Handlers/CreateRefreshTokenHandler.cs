﻿using System.Security.Cryptography;
using MediatR;
using Reactivities.Domain.Account.Models;
using Reactivities.Domain.Core;
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
                string token = GenerateRefreshToken();
                var resfreshToken = new RefreshToken()
                {
                    Token = token,
                    Expires = DateTime.UtcNow.AddMonths(3),
                    User = request.User
                };
                request.User.RefreshTokens.Add(resfreshToken);

                await _userRepository.Update(request.User);

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