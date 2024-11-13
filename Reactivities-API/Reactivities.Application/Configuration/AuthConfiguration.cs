namespace Reactivities.Application.Configuration
{
    public class AuthConfiguration
    {
        public string AccessTokenKey { get; set; } = string.Empty;
        public int AccessTokenLifetimeMinutes { get; set; }
        public int RefreshTokenLifetimeDays { get; set; }

        public TimeSpan AccessTokenLifetime => TimeSpan.FromMinutes(AccessTokenLifetimeMinutes);
        public TimeSpan RefreshTokenLifetime => TimeSpan.FromDays(RefreshTokenLifetimeDays);
    }
}
