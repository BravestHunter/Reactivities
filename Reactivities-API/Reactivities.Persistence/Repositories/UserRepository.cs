using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain.Core.Exceptions;
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

        public async Task<AppUser?> GetByUsernameWithPhotos(string username)
        {
            return await _userManager.Users
                .Include(u => u.Photos)
                .FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task<AppUser> Update(AppUser user)
        {
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                throw new FailedToUpdateEntityException("Faield to update user");
            }

            return user;
        }
    }
}
