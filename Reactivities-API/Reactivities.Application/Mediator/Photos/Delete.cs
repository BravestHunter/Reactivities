using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Interfaces;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Persistence;

namespace Reactivities.Application.Mediator.Photos
{
    public class Delete
    {
        public class Command : IRequest<Result>
        {
            public long Id { get; set; }
        }

        internal class Handler : IRequestHandler<Command, Result>
        {
            private readonly DataContext _dataContext;
            private readonly IPhotoAccessor _photoAccessor;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IPhotoAccessor photoAccessor, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
                _photoAccessor = photoAccessor;
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

                    if (photo.IsMain)
                    {
                        return Result.Failure("Main photo can't be deleted");
                    }

                    var deleteResult = await _photoAccessor.DeletePhoto(photo.StorageId);
                    if (deleteResult == null)
                    {
                        return Result.Failure("Failed to delete photo from Cloudinary");
                    }

                    user.Photos.Remove(photo);

                    var saveResult = await _dataContext.SaveChangesAsync() > 0;
                    if (!saveResult)
                    {
                        return Result.Failure("Failed to remove photo");
                    }

                    return Result.Success();
                }
                catch (Exception ex)
                {
                    return Result.Failure("Failed to remove photo", ex);
                }
            }
        }
    }
}
