using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Reactivities.Domain.Activities.Interfaces;

namespace Reactivities.Application.Security
{
    public class IsActivityHostRequirement : IAuthorizationRequirement
    {
    }

    public class IsActivityHostRequirementHandler : AuthorizationHandler<IsActivityHostRequirement>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IsActivityHostRequirementHandler(IActivityRepository activityRepository, IHttpContextAccessor httpContextAccessor)
        {
            _activityRepository = activityRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsActivityHostRequirement requirement)
        {
            var userIdStr = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userIdStr == null)
            {
                return;
            }
            var userId = long.Parse(userIdStr, CultureInfo.InvariantCulture);

            var activityIdStr = _httpContextAccessor.HttpContext?.Request.RouteValues.SingleOrDefault(x => x.Key == "id");
            if (!activityIdStr.HasValue || long.TryParse(activityIdStr.Value.ToString(), out long activityId))
            {
                return;
            }

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
