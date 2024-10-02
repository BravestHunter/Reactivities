using Reactivities.Api.Extensions;
using Reactivities.Api.Middleware;
using Reactivities.Api.SignalR;
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
            services.AddApplicationServices(configuration);
            services.AddIdentityServices(configuration);

            services.AddHealthChecks();
        }

        private static void ConfigurePipeline(WebApplication app)
        {
            app.UseHealthChecks("/healthCheck");

            // Configure the HTTP request pipeline.
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

            app.Services.ApplyPersistentMigrations();
            if (app.Environment.IsDevelopment())
            {
                app.Services.SeedPersistentData();
            }

            app.Run();
        }
    }
}
