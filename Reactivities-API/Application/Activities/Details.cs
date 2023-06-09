using Application.Core;
using Application.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Activities
{
    public class Details
    {
        public class Query : IRequest<Result<ActivityDto>>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Result<ActivityDto>>
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

            public async Task<Result<ActivityDto>> Handle(Query request, CancellationToken cancellationToken)
            {
                var username = _userAccessor.GetUsername();
                var activity = await _dataContext.Activities
                    .ProjectTo<ActivityDto>(_mapper.ConfigurationProvider, new { currentUsername = username })
                    .FirstOrDefaultAsync(a => a.Id == request.Id);

                return Result<ActivityDto>.Success(activity);
            }
        }
    }
}