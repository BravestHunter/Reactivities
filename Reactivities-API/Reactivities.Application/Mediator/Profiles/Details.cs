using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Interfaces;
using Reactivities.Persistence;

namespace Reactivities.Application.Mediator.Profiles
{
    public class Details
    {
        public class Query : IRequest<Result<Profile>>
        {
            public string Username { get; set; }
        }

        internal class Handler : IRequestHandler<Query, Result<Profile>>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;
            private readonly IMapper _mapper;

            public Handler(DataContext dataContext, IUserAccessor userAccessor, IMapper mapper)
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
                _mapper = mapper;
            }

            public async Task<Result<Profile>> Handle(Query request, CancellationToken cancellationToken)
            {
                try
                {
                    var username = _userAccessor.GetUsername();
                    var profile = await _dataContext.Users
                        .ProjectTo<Profile>(_mapper.ConfigurationProvider, new { currentUsername = username })
                        .SingleOrDefaultAsync(u => u.Username == request.Username);
                    if (profile == null)
                    {
                        return Result<Profile>.Failure("Failed to find profile");
                    }

                    return Result<Profile>.Success(profile);
                }
                catch (Exception ex)
                {
                    return Result<Profile>.Failure("Failed to get profile", ex);
                }
            }
        }
    }
}
