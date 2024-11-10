using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByUsername(string username);
        Task<AppUser?> GetByUsernameWithPhotos(string username);
        Task<ProfileDto?> GetProfileDto(string username, string currentUsername);
        Task<AppUser> Update(AppUser user);
    }
}
