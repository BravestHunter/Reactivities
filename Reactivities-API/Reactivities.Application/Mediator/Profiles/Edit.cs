using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Interfaces;
using Reactivities.Persistence;

namespace Reactivities.Application.Mediator.Profiles
{
    public class Edit
    {
        public class Command : IRequest<Result>
        {
            public string DisplayName { get; set; }
            public string Bio { get; set; }

        }

        internal class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.DisplayName).NotEmpty();
            }
        }

        internal class Handler : IRequestHandler<Command, Result>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _userAccessor = userAccessor;
            }

            public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    var username = _userAccessor.GetUsername();
                    var user = await _dataContext.Users.FirstOrDefaultAsync(u => u.UserName == username);
                    if (user == null)
                    {
                        return Result.Failure("Failed to find user");
                    }

                    user.DisplayName = request.DisplayName;
                    user.Bio = request.Bio;

                    var result = await _dataContext.SaveChangesAsync() > 0;
                    if (!result)
                    {
                        return Result.Failure("Failed to update profile");
                    }

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result.Failure("Failed to update profile", ex);
                }
            }
        }
    }
}
