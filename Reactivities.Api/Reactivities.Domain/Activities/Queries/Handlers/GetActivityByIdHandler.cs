﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;

namespace Reactivities.Domain.Activities.Queries.Handlers
{
    internal sealed class GetActivityByIdHandler : IRequestHandler<GetActivityByIdQuery, Result<ActivityDto>>
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public GetActivityByIdHandler(
            IActivityRepository activityRepository,
            IUserAccessor userAccessor,
            IMapper mapper,
            ILogger<GetActivityByIdHandler> logger
            )
        {
            _activityRepository = activityRepository;
            _userAccessor = userAccessor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<ActivityDto>> Handle(GetActivityByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();

                var activityDto = await _activityRepository.GetDtoById(request.Id, currentUsername);
                if (activityDto == null)
                {
                    return Result<ActivityDto>.Failure(new NotFoundException("Failed to find activity"));
                }

                return Result<ActivityDto>.Success(activityDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to get activity by id");
                return Result<ActivityDto>.Failure(ex);
            }
        }
    }
}
