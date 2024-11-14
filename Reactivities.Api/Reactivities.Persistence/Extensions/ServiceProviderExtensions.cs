using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Persistence.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static void ApplyPersistentMigrations(this IServiceProvider serviceProvider, ILogger logger)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                    dbContext.Database.MigrateAsync().Wait();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured during database migration");
            }
        }

        public static void SeedPersistentData(this IServiceProvider serviceProvider, ILogger logger)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                    Seed.SeedData(dbContext, userManager).Wait();
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occured during database seeding");
            }
        }
    }
}
