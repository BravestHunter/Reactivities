using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Users.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> IsUsernameTaken(string username);
        Task<bool> IsEmailTaken(string email);
        Task<AppUser?> GetByUsername(string username);
        Task<AppUser?> GetByUsernameWithPhotos(string username);
        Task<AppUser?> GetByRefreshToken(string refreshToken);
        Task<AppUser?> GetByEmail(string email);
        Task<ProfileDto?> GetProfileDto(string username, string currentUsername);
        Task<AppUser> Update(AppUser user);
    }
}
