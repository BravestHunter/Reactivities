using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Reactivities.Application.Exceptions;
using Reactivities.Domain.Core.Interfaces;

namespace Reactivities.Application.Security
{
    public class UserAccessor : IUserAccessor
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserAccessor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUsername()
        {
            return _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name) ?? throw new NotAuthorizedException();
        }
    }
}
