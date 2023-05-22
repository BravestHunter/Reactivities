using System.Security.Claims;
using API.DTO;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly TokenService _tokenService;

        public AccountController(UserManager<AppUser> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
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

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await _userManager.Users.AnyAsync(u => u.UserName == registerDto.Username))
            {
                ModelState.AddModelError("username", "Username is already taken");
                return ValidationProblem("Username is already taken");
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

        [Authorize]
        [HttpPost("refreshToken")]
        public async Task<ActionResult<UserDto>> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var user = await _userManager.Users
                .Include(u => u.RefreshTokens)
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == User.FindFirstValue(ClaimTypes.Name));
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
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.Email == email);

            await SetRefreshToken(user);

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
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
        }

        private UserDto CreateUserObject(AppUser user)
        {
            return new UserDto
            {
                Username = user.UserName,
                DisplayName = user.DisplayName,
                Image = user?.Photos?.FirstOrDefault(p => p.IsMain)?.Url,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}