﻿using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Reactivities.Domain.Activities.Interfaces;
using Reactivities.Domain.Comments.Dtos;
using Reactivities.Domain.Comments.Models;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Comments.Commands.Handlers
{
    internal sealed class CreateCommentHandler : IRequestHandler<CreateCommentCommand, Result<CommentDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IActivityRepository _activityRepository;
        private readonly IUserAccessor _userAccessor;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CreateCommentHandler(
            IUserRepository userRepository,
            IActivityRepository activityRepository,
            IUserAccessor userAccessor,
            IMapper mapper,
            ILogger<CreateCommentHandler> logger
            )
        {
            _userRepository = userRepository;
            _activityRepository = activityRepository;
            _userAccessor = userAccessor;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<CommentDto>> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();
                var user = await _userRepository.GetByUsername(currentUsername);
                if (user == null)
                {
                    return Result<CommentDto>.Failure(new NotFoundException("Failed to find current user"));
                }

                var activity = await _activityRepository.GetById(request.Comment.ActivityId);
                if (activity == null)
                {
                    return Result<CommentDto>.Failure(new NotFoundException("Failed to find activity"));
                }

                var comment = new Comment
                {
                    Author = user,
                    Activity = activity,
                    Body = request.Comment.Body
                };
                activity.Comments.Add(comment);

                await _activityRepository.Update(activity);

                _logger.LogInformation("Created comment {CommentId} for activity {ActivityId}", comment.Id, activity.Id);

                var commentDto = _mapper.Map<CommentDto>(comment);
                return Result<CommentDto>.Success(commentDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create comment");
                return Result<CommentDto>.Failure(ex);
            }
        }
    }
}
