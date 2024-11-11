using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Dtos;
using Reactivities.Application.Exceptions;
using Reactivities.Domain.Account.Commands;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Application.Services
{
    public class AccountService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMediator _mediator;

        public AccountService(IHttpContextAccessor httpContextAccessor, TokenService tokenService, UserManager<AppUser> userManager, IMediator mediator)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<Result<UserResponseDto>> Register(RegisterRequestDto registerDto)
        {
            try
            {
                var user = new AppUser
                {
                    UserName = registerDto.Username,
                    DisplayName = registerDto.DisplayName ?? registerDto.Username,
                    Email = registerDto.Email
                };

                var result = await _mediator.Send(new RegisterCommand() { User = user, Password = registerDto.Password });
                if (result.IsFailure)
                {
                    return Result<UserResponseDto>.Failure(result.Exception);
                }

                await SetRefreshTokenCookie(user);

                var userDto = CreateUserObject(user);
                return Result<UserResponseDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserResponseDto>.Failure(ex);
            }
        }

        public async Task<Result<UserResponseDto>> Login(LoginRequestDto loginDto)
        {
            try
            {
                var result = await _mediator.Send(new LoginCommand() { Email = loginDto.Email, Password = loginDto.Password });
                if (result.IsFailure)
                {
                    return Result<UserResponseDto>.Failure(result.Exception);
                }
                var user = result.GetOrThrow();

                await SetRefreshTokenCookie(user);

                var userDto = CreateUserObject(user);
                return Result<UserResponseDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserResponseDto>.Failure(ex);
            }
        }

        public async Task<Result<UserResponseDto>> RefreshToken()
        {
            try
            {
                var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];

                var user = await _userManager.Users
                    .Include(u => u.RefreshTokens)
                    .Include(u => u.Photos)
                    .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
                if (user == null)
                {
                    return Result<UserResponseDto>.Failure(new NotFoundException("Refresh token not found"));
                }

                var oldToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);

                if (oldToken != null && !oldToken.IsActive)
                {
                    return Result<UserResponseDto>.Failure(new NotFoundException("Failed to find valid refresh token"));
                }

                var userDto = CreateUserObject(user);
                return Result<UserResponseDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserResponseDto>.Failure(ex);
            }
        }

        public async Task<Result<UserResponseDto>> GetCurrentUser()
        {
            try
            {
                var email = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
                var user = await _userManager.Users
                    .Include(u => u.Photos)
                    .FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    return Result<UserResponseDto>.Failure(new NotFoundException("Failed to find current user"));
                }

                var userDto = CreateUserObject(user);
                return Result<UserResponseDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<UserResponseDto>.Failure(ex);
            }
        }

        private async Task SetRefreshTokenCookie(AppUser user)
        {
            var refreshTokenResult = await _mediator.Send(new CreateRefreshTokenCommand() { User = user });
            if (refreshTokenResult.IsFailure)
            {
                throw new AccountOperationFailedException("Failed to generate RefreshToken", refreshTokenResult.Exception);
            }
            var refreshToken = refreshTokenResult.GetOrThrow();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }

        private UserResponseDto CreateUserObject(AppUser user)
        {
            return new UserResponseDto
            {
                Username = user.UserName,
                DisplayName = user.DisplayName,
                Image = user?.Photos?.FirstOrDefault(p => p.IsMain)?.Url,
                AccessToken = _tokenService.CreateAccessToken(user)
            };
        }
    }
}
