using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Account.Dtos;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Users.Interfaces;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Account.Commands.Handlers
{
    internal sealed class LoginHandler : IRequestHandler<LoginCommand, Result<CurrentUserDto>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public LoginHandler(UserManager<AppUser> userManager, IUserRepository userRepository, IMapper mapper, ILogger<LoginHandler> logger)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<CurrentUserDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userRepository.GetByEmailWithProfilePhoto(request.Email);
                if (user == null)
                {
                    return Result<CurrentUserDto>.Failure(new NotFoundException("Failed to find user"));
                }

                var result = await _userManager.CheckPasswordAsync(user, request.Password);
                if (!result)
                {
                    return Result<CurrentUserDto>.Failure(new BadRequestException("Failed to login"));
                }

                _logger.LogInformation("Logged in user {UserName}", user.UserName);

                var userDto = _mapper.Map<CurrentUserDto>(user);
                return Result<CurrentUserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to login");
                return Result<CurrentUserDto>.Failure(ex);
            }
        }
    }
}
