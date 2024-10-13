using Reactivities.Api.Extensions;
using Reactivities.Api.Middleware;
using Reactivities.Api.SignalR;
using Reactivities.Application.Extensions;
using Reactivities.Persistence.Extensions;

namespace Reactivities.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder.Services, builder.Configuration);

            var app = builder.Build();

            ConfigurePipeline(app);

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddPersistanceServices();
            services.AddApplicationServices();
            services.AddIdentityServices(configuration);
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

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.MapControllers();
            app.MapHub<ChatHub>("/chat");
            app.MapFallbackToController("Index", "Fallback");

            app.Services.ApplyPersistentMigrations(app.Logger);
            if (app.Environment.IsDevelopment())
            {
                app.Services.SeedPersistentData(app.Logger);
            }

            app.Run();
        }
    }
}
