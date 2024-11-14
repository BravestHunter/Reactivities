using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Interfaces;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Persistence.Repositories
{
    internal sealed class UserFollowingRepository : IUserFollowingRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserFollowingRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UserFollowing?> GetByIds(long observerId, long targetId)
        {
            return await _context.UserFollowings.FindAsync(observerId, targetId);
        }

        public async Task<List<ProfileDto>> GetFollowerDtos(string username, string currentUsername)
        {
            return await _context.UserFollowings
                .Where(u => u.Target.UserName == username)
                .Select(u => u.Observer)
                .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider, new { currentUsername })
                .ToListAsync();
        }

        public async Task<List<ProfileDto>> GetFollowingDtos(string username, string currentUsername)
        {
            return await _context.UserFollowings
                .Where(u => u.Observer.UserName == username)
                .Select(u => u.Target)
                .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider, new { currentUsername })
                .ToListAsync();
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
