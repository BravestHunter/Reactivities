using Application.Core;
using Application.Interfaces;
using AutoMapper;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Comments
{
    public class Create
    {
        public class Command : IRequest<Result<CommentDto>>
        {
            public string Body { get; set; }
            public Guid ActivityId { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Body).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, Result<CommentDto>>
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

            public async Task<Result<CommentDto>> Handle(Command request, CancellationToken cancellationToken)
            {
                var username = _userAccessor.GetUsername();
                var user = await _dataContext.Users
                    .Include(u => u.Photos)
                    .SingleOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    return null;
                }

                var activity = await _dataContext.Activities.FindAsync(request.ActivityId);
                if (activity == null)
                {
                    return null;
                }

                var comment = new Comment
                {
                    Author = user,
                    Activity = activity,
                    Body = request.Body
                };
                activity.Comments.Add(comment);

                var result = await _dataContext.SaveChangesAsync() > 0;
                if (!result)
                {
                    return Result<CommentDto>.Failure("Failed to create comment");
                }

                return Result<CommentDto>.Success(_mapper.Map<CommentDto>(comment));
            }
        }
    }
}