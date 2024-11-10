using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Comments.Interfaces;
using Reactivities.Domain.Users.Interfaces;
using Reactivities.Persistence.Repositories;

namespace Reactivities.Persistence.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services)
        {
            services.AddDbContext<DataContext>((serviceProvider, options) =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                string? connectionString = configuration.GetConnectionString("PostgresDb");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ApplicationException("Failed to retrieve PostgresDb connection string");
                }

                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IActivityRepository, ActivityRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserFollowingRepository, UserFollowingRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();

            return services;
        }
    }
}
