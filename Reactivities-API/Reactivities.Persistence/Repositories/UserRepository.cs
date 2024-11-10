﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Interfaces;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Persistence.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public UserRepository(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
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

        public async Task<ProfileDto?> GetProfileDto(string username, string currentUsername)
        {
            return await _userManager.Users
                .ProjectTo<ProfileDto>(_mapper.ConfigurationProvider, new { currentUsername })
                .SingleOrDefaultAsync(u => u.Username == username);
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
