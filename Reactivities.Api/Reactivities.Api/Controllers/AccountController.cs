using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Reactivities.Application.Dtos;
using Reactivities.Application.Services;

namespace Reactivities.Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService, IMediator mediator) : base(mediator)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterRequestDto registerDto)
        {
            var result = await _accountService.Register(registerDto);
            if (result.IsFailure)
            {
                return HandleResult(result);
            }

            return Ok(result.GetOrThrow());
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginRequestDto loginDto)
        {
            var result = await _accountService.Login(loginDto);
            if (result.IsFailure)
            {
                return Unauthorized();
            }

            return Ok(result.GetOrThrow());
        }

        [HttpGet("refreshToken")]
        [AllowAnonymous]
        public async Task<ActionResult> RefreshToken()
        {
            var result = await _accountService.RefreshToken();
            if (result.IsFailure)
            {
                return Unauthorized();
            }

            return Ok(result.GetOrThrow());
        }

        [HttpGet]
        public async Task<ActionResult> GetCurrentUser()
        {
            var result = await _accountService.GetCurrentUser();
            if (result.IsFailure)
            {
                return HandleResult(result);
            }

            return Ok(result.GetOrThrow());
        }
    }
}
