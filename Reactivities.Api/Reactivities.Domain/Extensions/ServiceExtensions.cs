using System.Reflection;
using System.Security.Cryptography;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(thisAssembly);
            services.AddValidatorsFromAssembly(thisAssembly);
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(thisAssembly);
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddSingleton(serviceProvider => RandomNumberGenerator.Create());

            return services;
        }
    }
}
