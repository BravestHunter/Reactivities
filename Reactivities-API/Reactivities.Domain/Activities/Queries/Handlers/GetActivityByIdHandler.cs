using AutoMapper;
using MediatR;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Interfaces;

namespace Reactivities.Domain.Activities.Queries.Handlers
{
    internal class GetActivityByIdHandler : IRequestHandler<GetActivityByIdQuery, Result<ActivityDto>>
    {
        private readonly IActivityRepository _repository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public GetActivityByIdHandler(IActivityRepository repository, IUserAccessor userAccessor, IMapper mapper)
        {
            _repository = repository;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result<ActivityDto>> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();

                var activityDto = await _repository.GetDtoById(request.Id, currentUsername);
                if (activityDto == null)
                {
                    return Result<ActivityDto>.Failure("Failed to get activity from storage");
                }

                return Result<ActivityDto>.Success(activityDto);
            }
            catch (Exception ex)
            {
                return Result<ActivityDto>.Failure(ex);
            }
        }
    }
}
