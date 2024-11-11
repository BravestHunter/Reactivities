using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reactivities.Api.Dto;
using Reactivities.Api.DTO;
using Reactivities.Application.Services;
using Reactivities.Domain.Account.Commands;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService, IMediator mediator) : base(mediator)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponseDto>> Register(RegisterRequestDto registerDto)
        {
            var user = new AppUser
            {
                UserName = registerDto.Username,
                DisplayName = registerDto.DisplayName ?? registerDto.Username,
                Email = registerDto.Email
            };

            var result = await Mediator.Send(new RegisterCommand() { User = user, Password = registerDto.Password });
            if (result.IsFailure)
            {
                return HandleResult(result);
            }

            await SetRefreshTokenCookie(user);

            return CreateUserObject(user);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponseDto>> Login(LoginRequestDto loginDto)
        {
            var result = await Mediator.Send(new LoginCommand() { Email = loginDto.Email, Password = loginDto.Password });
            if (result.IsFailure)
            {
                return Unauthorized();
            }
            var user = result.GetOrThrow();

            await SetRefreshTokenCookie(user);

            return CreateUserObject(user);
        }

        [HttpGet("refreshToken")]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponseDto>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
            if (user == null)
            {
                return Unauthorized();
            }

            var oldToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);

            if (oldToken != null && !oldToken.IsActive)
            {
                return Unauthorized();
            }

            return CreateUserObject(user);
        }

        [HttpGet]
        public async Task<ActionResult<UserResponseDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.Email == email);

            return CreateUserObject(user);
        }

        private async Task SetRefreshTokenCookie(AppUser user)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshToken.Expires
            };

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
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
