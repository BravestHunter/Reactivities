using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Reactivities.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers(opt =>
            {
                // Make all the controlelrs require authorized access by default
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            });

            var webClientAddresses = config.GetSection("WebClientAddress").GetChildren()
                .Select(x => x.Value ?? string.Empty)
                .Where(string.IsNullOrWhiteSpace)
                .ToArray();
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowCredentials()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("WWW-Authenticate")
                        .WithOrigins(webClientAddresses);
                });
            });

            services.AddHttpContextAccessor();

            services.AddSignalR();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddHealthChecks();

            return services;
        }
    }
}
