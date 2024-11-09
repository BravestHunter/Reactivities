using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Reactivities.Api.Configuration;
using Reactivities.Api.Services;
using Reactivities.Application.Interfaces;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Models;
using Reactivities.Infrastructure.Photos;
using Reactivities.Infrastructure.Security;
using Reactivities.Persistence;

namespace Reactivities.Api.Extensions
{
    public static class ServiceExtensions
    {
        public const string ConfigAuthorizationSection = "Authorization";

        public static IServiceCollection BindAuthConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AuthConfiguration>(config.GetSection(ConfigAuthorizationSection));

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
                .AddEntityFrameworkStores<DataContext>();

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
                opt.AddPolicy("IsActivityHost", policy =>
                {
                    policy.Requirements.Add(new IsHostRequirement());
                });
            });
            services.AddTransient<IAuthorizationHandler, IsHostRequirementHandler>();

            services.AddScoped<TokenService>();

            return services;
        }

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

            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", policy =>
                {
                    policy
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("WWW-Authenticate", "Pagination")
                        .WithOrigins("http://localhost:3000", "https://localhost:3000");
                });
            });

            services.AddHttpContextAccessor();
            services.AddScoped<IUserAccessor, UserAccessor>();

            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));

            services.AddSignalR();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddHealthChecks();

            return services;
        }
    }
}
