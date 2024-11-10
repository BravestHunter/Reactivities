using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByUsername(string username);
    }
}
