﻿using AutoMapper;
using MediatR;
using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Interfaces;

namespace Reactivities.Domain.Activities.Commands.Handlers
{
    internal class EditActivityHandler : IRequestHandler<EditActivityCommand, Result<ActivityDto>>
    {
        private readonly IActivityRepository _repository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;

        public EditActivityHandler(IActivityRepository repository, IUserAccessor userAccessor, IMapper mapper)
        {
            _repository = repository;
            _userAccessor = userAccessor;
            _mapper = mapper;
        }

        public async Task<Result<ActivityDto>> Handle(EditActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingActivity = await _repository.GetById(request.Activity.Id);
                if (existingActivity == null)
                {
                    return Result<ActivityDto>.Failure("Failed to find activity");
                }

                _mapper.Map(request.Activity, existingActivity);

                var activityDto = await _repository.Update(existingActivity);
                return Result<ActivityDto>.Success(activityDto);
            }
            catch (Exception ex)
            {
                return Result<ActivityDto>.Failure(ex);
            }
        }
    }
}