using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Reactivities.Persistence.Extensions
{
    public static class IdentityBuilderExtensions
    {
        public static IdentityBuilder AddPersistenceIdentityStores(this IdentityBuilder identityBuilder)
        {
            identityBuilder.AddEntityFrameworkStores<DataContext>();

            return identityBuilder;
        }
    }
}
