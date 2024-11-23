using AutoMapper;
using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Users.Queries.Handlers
{
    internal sealed class GetFollowingListHandler : IRequestHandler<GetFollowingListQuery, Result<List<ProfileDto>>>
    {
        private readonly IUserFollowingRepository _userFollowingRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public GetFollowingListHandler(IUserFollowingRepository userFollowingRepository, IUserAccessor userAccessor, IMapper mapper)
        {
            _userFollowingRepository = userFollowingRepository;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result<List<ProfileDto>>> Handle(GetFollowingListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();
                var followers = await _userFollowingRepository.GetFollowingDtos(request.Username, currentUsername);

                return Result<List<ProfileDto>>.Success(followers);
            }
            catch (Exception ex)
            {
                return Result<List<ProfileDto>>.Failure(ex);
            }
        }
    }
}
