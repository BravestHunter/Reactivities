using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Reactivities.Application.Configuration;
using Reactivities.Application.Dtos;
using Reactivities.Application.Exceptions;
using Reactivities.Domain.Account.Commands;
using Reactivities.Domain.Account.Dtos;
using Reactivities.Domain.Account.Queries;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Application.Services
{
    public class AccountService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMediator _mediator;
        private readonly AuthConfiguration _authConfig;

        public AccountService(IHttpContextAccessor httpContextAccessor, IMediator mediator, IOptions<AuthConfiguration> authOptions)
        {
            _httpContextAccessor = httpContextAccessor;
            _mediator = mediator;
            _authConfig = authOptions.Value;
        }

        public async Task<Result<CurrentUserDto>> Register(RegisterRequestDto registerDto)
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
                    return Result<CurrentUserDto>.Failure(result.Exception);
                }
                var userDto = result.GetOrThrow();

                await SetRefreshTokenCookie(userDto);

                return Result<CurrentUserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<CurrentUserDto>.Failure(ex);
            }
        }

        public async Task<Result<CurrentUserDto>> Login(LoginRequestDto loginDto)
        {
            try
            {
                var result = await _mediator.Send(new LoginCommand() { Email = loginDto.Email, Password = loginDto.Password });
                if (result.IsFailure)
                {
                    return Result<CurrentUserDto>.Failure(result.Exception);
                }
                var userDto = result.GetOrThrow();

                await SetRefreshTokenCookie(userDto);

                return Result<CurrentUserDto>.Success(userDto);
            }
            catch (Exception ex)
            {
                return Result<CurrentUserDto>.Failure(ex);
            }
        }

        public async Task<Result<AccessTokenResponseDto>> RefreshToken()
        {
            try
            {
                var refreshToken = _httpContextAccessor.HttpContext?.Request.Cookies["refreshToken"];
                if (string.IsNullOrWhiteSpace(refreshToken))
                {
                    return Result<AccessTokenResponseDto>.Failure(new BadRequestException("Failed to get RefreshToken cookie"));
                }

                var result = await _mediator.Send(new RefreshAccessTokenCommand() { RefreshToken = refreshToken });
                if (result.IsFailure)
                {
                    return Result<AccessTokenResponseDto>.Failure(result.Exception);
                }
                var accessToken = result.GetOrThrow();

                var accessTokenDto = new AccessTokenResponseDto() { AccessToken = accessToken };
                return Result<AccessTokenResponseDto>.Success(accessTokenDto);
            }
            catch (Exception ex)
            {
                return Result<AccessTokenResponseDto>.Failure(ex);
            }
        }

        public async Task<Result<CurrentUserDto>> GetCurrentUser()
        {
            try
            {
                return await _mediator.Send(new GetCurrentUserQuery());
            }
            catch (Exception ex)
            {
                return Result<CurrentUserDto>.Failure(ex);
            }
        }

        private async Task SetRefreshTokenCookie(CurrentUserDto user)
        {
            var refreshTokenResult = await _mediator.Send(new CreateRefreshTokenCommand()
            {
                Username = user.Username,
                RefreshTokenLifetime = _authConfig.RefreshTokenLifetime
            });
            if (refreshTokenResult.IsFailure)
            {
                throw new AccountOperationFailedException("Failed to generate RefreshToken", refreshTokenResult.Exception);
            }
            var refreshToken = refreshTokenResult.GetOrThrow();

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
                Secure = true
            };

            _httpContextAccessor.HttpContext?.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }
    }
}
