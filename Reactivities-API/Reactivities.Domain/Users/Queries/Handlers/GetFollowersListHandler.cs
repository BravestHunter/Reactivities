using AutoMapper;
using MediatR;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Dtos;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Users.Queries.Handlers
{
    internal class GetFollowersListHandler : IRequestHandler<GetFollowersListQuery, Result<List<ProfileDto>>>
    {
        private readonly IUserFollowingRepository _repository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public GetFollowersListHandler(IUserFollowingRepository repository, IUserAccessor userAccessor, IMapper mapper)
        {
            _repository = repository;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result<List<ProfileDto>>> Handle(GetFollowersListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();
                var followers = await _repository.GetFollowerDtos(request.Username, currentUsername);

                return Result<List<ProfileDto>>.Success(followers);
            }
            catch (Exception ex)
            {
                return Result<List<ProfileDto>>.Failure(ex);
            }
        }
    }
}
