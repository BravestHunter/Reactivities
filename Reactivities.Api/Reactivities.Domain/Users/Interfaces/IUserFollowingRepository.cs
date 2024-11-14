using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Users.Interfaces
{
    public interface IUserFollowingRepository
    {
        Task<UserFollowing?> GetByIds(long observerId, long targetId);
        Task<List<ProfileDto>> GetFollowerDtos(string username, string currentUsername);
        Task<List<ProfileDto>> GetFollowingDtos(string username, string currentUsername);
        Task Add(UserFollowing following);
        Task Delete(UserFollowing following);
    }
}
