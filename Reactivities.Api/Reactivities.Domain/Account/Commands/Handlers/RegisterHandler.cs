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
    internal sealed class RegisterHandler(UserManager<AppUser> userManager, IUserRepository userRepository, IMapper mapper, ILogger<RegisterHandler> logger) : IRequestHandler<RegisterCommand, Result<CurrentUserDto>>
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger _logger = logger;

        public async Task<Result<CurrentUserDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.User.UserName != null && !await _userRepository.IsUsernameTaken(request.User.UserName))
                {
                    return Result<CurrentUserDto>.Failure(new BadRequestException("Username is already taken"));
                }
                if (request.User.Email != null && !await _userRepository.IsEmailTaken(request.User.Email))
                {
                    return Result<CurrentUserDto>.Failure(new BadRequestException("Email is already taken"));
                }

                var result = await _userManager.CreateAsync(request.User, request.Password);
                if (!result.Succeeded)
                {
                    return Result<CurrentUserDto>.Failure(new BadRequestException($"Errors: {string.Join("; ", result.Errors)}"));
                }

                _logger.LogInformation("Registered user {UserName}", request.User.UserName);

                var userDto = _mapper.Map<CurrentUserDto>(request.User);
                return Result<CurrentUserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to register user");
                return Result<CurrentUserDto>.Failure(ex);
            }
        }
    }
}
