using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Reactivities.Persistence.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services)
        {
            services.AddDbContext<DataContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                string connectionString = configuration.GetConnectionString("PostgresDb") ?? string.Empty;
                options.UseNpgsql(connectionString);
            });

            return services;
        }
    }
}
