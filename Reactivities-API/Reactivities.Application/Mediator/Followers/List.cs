using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Persistence;

namespace Reactivities.Application.Followers
{
    public class List
    {
        public class Query : IRequest<Result<List<Mediator.Profiles.Profile>>>
        {
            public string Predicate { get; set; }
            public string Username { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<List<Mediator.Profiles.Profile>>>
        {
            private readonly DataContext _dataContext;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IMapper mapper, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _mapper = mapper;
                _userAccessor = userAccessor;
            }

            public async Task<Result<List<Mediator.Profiles.Profile>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var profiles = new List<Mediator.Profiles.Profile>();

                var username = _userAccessor.GetUsername();

                switch (request.Predicate)
                {
                    case "followers":
                        profiles = await _dataContext.UserFollowings
                            .Where(u => u.Target.UserName == request.Username)
                            .Select(u => u.Observer)
                            .ProjectTo<Mediator.Profiles.Profile>(_mapper.ConfigurationProvider,
                                new { currentUsername = username })
                            .ToListAsync();
                        break;

                    case "following":
                        profiles = await _dataContext.UserFollowings
                            .Where(u => u.Observer.UserName == request.Username)
                            .Select(u => u.Target)
                            .ProjectTo<Mediator.Profiles.Profile>(_mapper.ConfigurationProvider,
                                new { currentUsername = username })
                            .ToListAsync();
                        break;
                }

                return Result<List<Mediator.Profiles.Profile>>.Success(profiles);
            }
        }
    }
}
