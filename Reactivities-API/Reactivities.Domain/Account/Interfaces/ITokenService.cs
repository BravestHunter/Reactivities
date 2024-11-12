using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Account.Interfaces
{
    public interface ITokenService
    {
        string GenerateAccessToken(AppUser user);
    }
}
