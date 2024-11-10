﻿using AutoMapper;
using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Users.Queries.Handlers
{
    internal class GetFollowingListHandler : IRequestHandler<GetFollowingListQuery, Result<List<ProfileDto>>>
    {
        private readonly IUserFollowingRepository _repository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public GetFollowingListHandler(IUserFollowingRepository repository, IUserAccessor userAccessor, IMapper mapper)
        {
            _repository = repository;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result<List<ProfileDto>>> Handle(GetFollowingListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();
                var followers = await _repository.GetFollowingDtos(request.Username, currentUsername);

                return Result<List<ProfileDto>>.Success(followers);
            }
            catch (Exception ex)
            {
                return Result<List<ProfileDto>>.Failure(ex);
            }
        }
    }
}
