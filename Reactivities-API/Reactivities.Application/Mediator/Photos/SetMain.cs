using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Interfaces;
using Reactivities.Persistence;

namespace Reactivities.Application.Mediator.Photos
{
    public class SetMain
    {
        public class Command : IRequest<Result>
        {
            public long Id { get; set; }
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
                    string username = _userAccessor.GetUsername();
                    var user = await _dataContext.Users
                        .Include(u => u.Photos)
                        .FirstOrDefaultAsync(u => u.UserName == username);
                    if (user == null)
                    {
                        return Result.Failure("Failed to find user");
                    }

                    var photo = user.Photos.FirstOrDefault(p => p.Id == request.Id);
                    if (photo == null)
                    {
                        return Result.Failure("Failed to find photo");
                    }

                    var mainPhoto = user.Photos.FirstOrDefault(p => p.IsMain);
                    if (mainPhoto != null)
                    {
                        mainPhoto.IsMain = false;
                    }

                    photo.IsMain = true;

                    var saveResult = await _dataContext.SaveChangesAsync() > 0;
                    if (!saveResult)
                    {
                        return Result.Failure("Failed to set main photo");
                    }

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result.Failure("Failed to set main photo", ex);
                }
            }
        }
    }
}
