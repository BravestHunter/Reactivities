using Microsoft.AspNetCore.Identity;
using Reactivities.Domain.Users.Interfaces;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Persistence.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;

        public UserRepository(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<AppUser?> GetByUsername(string username)
        {
            return await _userManager.FindByNameAsync(username);
        }
    }
}
