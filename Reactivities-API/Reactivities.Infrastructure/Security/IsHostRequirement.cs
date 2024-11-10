using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Reactivities.Domain.Activities.Interfaces;

namespace Reactivities.Infrastructure.Security
{
    public class IsHostRequirement : IAuthorizationRequirement
    {

    }

    public class IsHostRequirementHandler : AuthorizationHandler<IsHostRequirement>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsHostRequirementHandler(IActivityRepository activityRepository, IHttpContextAccessor httpContextAccessor)
        {
            _activityRepository = activityRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsHostRequirement requirement)
        {
            var userIdStr = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
            {
                return;
            }
            var userId = long.Parse(userIdStr);

            var activityId = long.Parse(
                _httpContextAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id").Value?.ToString()
                );

            var activity = await _activityRepository.GetByIdWithHost(activityId);
            if (activity == null)
            {
                return;
            }

            if (activity.Host.Id == userId)
            {
                context.Succeed(requirement);
            }
        }
    }
}
