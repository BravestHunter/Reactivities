using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reactivities.Api.Dto;
using Reactivities.Api.DTO;
using Reactivities.Api.Services;
using Reactivities.Persistence.Models;

namespace Reactivities.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserResponseDto>> Register(RegisterRequestDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(u => u.UserName == registerDto.Username))
            {
                ModelState.AddModelError("username", "Username is already taken");
                return ValidationProblem(ModelState);
            }

            if (await _userManager.Users.AnyAsync(u => u.Email == registerDto.Email))
            {
                ModelState.AddModelError("email", "Email is already taken");
                return ValidationProblem(ModelState);
            }

            var user = new AppUser
            {
                UserName = registerDto.Username,
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            await SetRefreshToken(user);

            return CreateUserObject(user);
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserResponseDto>> Login(LoginRequestDto loginDto)
        {
            var user = await _userManager.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
            {
                return Unauthorized();
            }

            await SetRefreshToken(user);

            return CreateUserObject(user);
        }

        [HttpGet("refreshToken")]
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
        [Authorize]
        public async Task<ActionResult<UserResponseDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.Email == email);

            return CreateUserObject(user);
        }

        private async Task SetRefreshToken(AppUser user)
        {
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(user);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(30)
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
