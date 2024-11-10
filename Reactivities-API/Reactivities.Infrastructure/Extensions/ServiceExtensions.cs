using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reactivities.Domain.Photos.Interfaces;
using Reactivities.Infrastructure.Configuration;
using Reactivities.Infrastructure.Photos;

namespace Reactivities.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<CloudinarySettings>(config.GetSection("Cloudinary"));
            services.AddScoped<IPhotoStorage, CloudinaryPhotoStorage>();

            return services;
        }
    }
}
