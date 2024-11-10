﻿using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Users.Dtos;

namespace Reactivities.Domain.Users.Queries
{
    public class GetFollowingListQuery : IRequest<Result<List<ProfileDto>>>
    {
        public required string Username { get; set; }
    }
}
