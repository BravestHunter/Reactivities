using MediatR;
using Microsoft.AspNetCore.Identity;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Users.Interfaces;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Account.Commands.Handlers
{
    internal class LoginHandler : IRequestHandler<LoginCommand, Result<AppUser>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;

        public LoginHandler(UserManager<AppUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<Result<AppUser>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByEmail(request.Email);
                if (user == null)
                {
                    return Result<AppUser>.Failure(new NotFoundException("Failed to find user"));
                }

                var result = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!result)
                {
                    return Result<AppUser>.Failure(new BadRequestException("Failed to login"));
                }

                return Result<AppUser>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<AppUser>.Failure(ex);
            }
        }
    }
}
