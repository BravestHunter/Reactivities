using Reactivities.Domain.Users.Interfaces;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Persistence.Repositories
{
    internal class UserFollowingRepository : IUserFollowingRepository
    {
        private readonly DataContext _context;

        public UserFollowingRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<UserFollowing?> GetByIds(long observerId, long targetId)
        {
            return await _context.UserFollowings.FindAsync(observerId, targetId);
        }

        public async Task Add(UserFollowing following)
        {
            _context.UserFollowings.Add(following);
            await SaveChanges();
        }

        public async Task Delete(UserFollowing following)
        {
            _context.UserFollowings.Remove(following);
            await SaveChanges();
        }

        private async Task SaveChanges()
        {
            var result = await _context.SaveChangesAsync() > 0;
            if (!result)
            {
                throw new InvalidOperationException("Failed to save persistent changes");
            }
        }
    }
}
