using MediatR;
using Microsoft.AspNetCore.Identity;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Users.Interfaces;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Account.Commands.Handlers
{
    internal class RegisterHandler : IRequestHandler<RegisterCommand, Result<AppUser>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;

        public RegisterHandler(UserManager<AppUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<Result<AppUser>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.User.UserName != null && !await _userRepository.IsUsernameTaken(request.User.UserName))
                {
                    return Result<AppUser>.Failure(new BadRequestException("Username is already taken"));
                }
                if (request.User.Email != null && !await _userRepository.IsEmailTaken(request.User.Email))
                {
                    return Result<AppUser>.Failure(new BadRequestException("Email is already taken"));
                }

                var result = await _userManager.CreateAsync(request.User, request.Password);
                if (!result.Succeeded)
                {
                    return Result<AppUser>.Failure(new BadRequestException($"Errors: {string.Join("; ", result.Errors)}"));
                }

                return Result<AppUser>.Success(request.User);
            }
            catch (Exception ex)
            {
                return Result<AppUser>.Failure(ex);
            }
        }
    }
}
