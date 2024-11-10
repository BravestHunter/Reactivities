using Reactivities.Api.Extensions;
using Reactivities.Api.Middleware;
using Reactivities.Api.SignalR;
using Reactivities.Domain.Extensions;
using Reactivities.Infrastructure.Extensions;
using Reactivities.Persistence.Extensions;

namespace Reactivities.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            BindConfiguration(builder.Services, builder.Configuration);
            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            ConfigurePipeline(app);

            app.Run();
        }

        private static void BindConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            services.BindAuthConfiguration(configuration);
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDomainServices();
            services.AddPersistanceServices();
            services.AddInfrastructureServices(configuration);
            services.AddIdentityServices();
            services.AddApiServices(configuration);
        }

        private static void ConfigurePipeline(WebApplication app)
        {
            app.UseHealthChecks("/healthCheck");

            app.UseMiddleware<ExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<ChatHub>("/chat");

            app.Services.ApplyPersistentMigrations(app.Logger);
            if (app.Environment.IsDevelopment())
            {
                app.Services.SeedPersistentData(app.Logger);
            }

            app.Run();
        }
    }
}
