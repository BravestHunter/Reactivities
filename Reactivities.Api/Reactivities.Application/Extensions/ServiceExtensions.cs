using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Reactivities.Application.Configuration;
using Reactivities.Application.Security;
using Reactivities.Application.Services;
using Reactivities.Domain.Account.Interfaces;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Models;
using Reactivities.Persistence.Extensions;

namespace Reactivities.Application.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection BindAuthConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AuthConfiguration>(config.GetSection("Authorization"));

            return services;
        }

        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            using var serviceProvider = services.BuildServiceProvider();

            services
                .AddIdentityCore<AppUser>(opt =>
                {
                    opt.User.RequireUniqueEmail = true;
                    opt.Password.RequireNonAlphanumeric = false;
                })
                .AddPersistenceIdentityStores();

            var authConfig = serviceProvider.GetRequiredService<IOptions<AuthConfiguration>>().Value;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.AccessTokenKey));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer((opt) =>
                {
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization(opt =>
            {
                opt.AddPolicy(AuthorizePolicies.IsActivityHost, policy =>
                {
                    policy.Requirements.Add(new IsActivityHostRequirement());
                });
            });
            services.AddTransient<IAuthorizationHandler, IsActivityHostRequirementHandler>();

            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<AccountService>();

            return services;
        }
    }
}
