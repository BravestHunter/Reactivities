using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Interfaces;
using Reactivities.Persistence;

namespace Reactivities.Application.Photos
{
    public class Delete
    {
        public class Command : IRequest<Result<Unit>>
        {
            public string Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
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

            public async Task<Result<Unit>> Handle(Command request, CancellationToken cancellationToken)
            {
                string username = _userAccessor.GetUsername();
                var user = await _dataContext.Users
                    .Include(u => u.Photos)
                    .FirstOrDefaultAsync(u => u.UserName == username);
                if (user == null)
                {
                    return null;
                }

                var photo = user.Photos.FirstOrDefault(p => p.Id == request.Id);
                if (photo == null)
                {
                    return null;
                }

                if (photo.IsMain)
                {
                    return Result<Unit>.Failure("Main photo can't be deleted");
                }

                var deleteResult = await _photoAccessor.DeletePhoto(photo.Id);
                if (deleteResult == null)
                {
                    return Result<Unit>.Failure("Failed to delete photo from Cloudinary");
                }

                user.Photos.Remove(photo);

                var saveResult = await _dataContext.SaveChangesAsync() > 0;
                if (!saveResult)
                {
                    return Result<Unit>.Failure("Failed to remove photo");
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
