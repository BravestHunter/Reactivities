using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Interfaces;
using Reactivities.Persistence;
using Reactivities.Persistence.Models;

namespace Reactivities.Application.Mediator.Photos
{
    public class Add
    {
        public class Command : IRequest<Result<Photo>>
        {
            public IFormFile File { get; set; }
        }

        internal class Handler : IRequestHandler<Command, Result<Photo>>
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

            public async Task<Result<Photo>> Handle(Command request, CancellationToken cancellationToken)
            {
                try
                {
                    string username = _userAccessor.GetUsername();
                    var user = await _dataContext.Users
                        .Include(u => u.Photos)
                        .FirstOrDefaultAsync(u => u.UserName == username);
                    if (user == null)
                    {
                        return Result<Photo>.Failure("Failed to find user");
                    }

                    var uploadResult = await _photoAccessor.AddPhoto(request.File);

                    var photo = new Photo
                    {
                        Url = uploadResult.Url,
                        StorageId = uploadResult.PublicId
                    };

                    if (!user.Photos.Any())
                    {
                        photo.IsMain = true;
                    }

                    user.Photos.Add(photo);

                    var saveResult = await _dataContext.SaveChangesAsync() > 0;
                    if (!saveResult)
                    {
                        return Result<Photo>.Failure("Failed to add a photo");
                    }

                    return Result<Photo>.Success(photo);
                }
                catch (Exception ex)
                {
                    return Result<Photo>.Failure("Failed to add a photo", ex);
                }
            }
        }
    }
}
