using System.Reflection;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Reactivities.Domain.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            var thisAssembly = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(thisAssembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(thisAssembly);
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(thisAssembly);
            });

            return services;
        }
    }
}
