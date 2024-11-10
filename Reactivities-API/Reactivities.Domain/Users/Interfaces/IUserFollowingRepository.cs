using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Users.Interfaces
{
    public interface IUserFollowingRepository
    {
        Task<UserFollowing?> GetByIds(long observerId, long targetId);
        Task Add(UserFollowing following);
        Task Delete(UserFollowing following);
    }
}
