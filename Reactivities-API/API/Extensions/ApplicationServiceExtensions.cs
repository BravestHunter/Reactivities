using Application.Activities;
using Application.Core;
using Application.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Photos;
using Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Persistence.Extensions;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddPersistanceServices(config);

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<List.Handler>();
            });
            services.AddAutoMapper(typeof(MappingProfiles).Assembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Create>();

            services.AddControllers(opt =>
            {
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

            return services;
        }
    }
}
