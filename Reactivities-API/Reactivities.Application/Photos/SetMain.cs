using MediatR;
using Microsoft.EntityFrameworkCore;
using Reactivities.Application.Core;
using Reactivities.Application.Interfaces;
using Reactivities.Persistence;

namespace Reactivities.Application.Photos
{
    public class SetMain
    {
        public class Command : IRequest<Result<Unit>>
        {
            public long Id { get; set; }
        }

        public class Handler : IRequestHandler<Command, Result<Unit>>
        {
            private readonly DataContext _dataContext;
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, IUserAccessor userAccessor)
            {
                _dataContext = dataContext;
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

                var mainPhoto = user.Photos.FirstOrDefault(p => p.IsMain);
                if (mainPhoto != null)
                {
                    mainPhoto.IsMain = false;
                }

                photo.IsMain = true;

                var saveResult = await _dataContext.SaveChangesAsync() > 0;
                if (!saveResult)
                {
                    return Result<Unit>.Failure("Failed to set main photo");
                }

                return Result<Unit>.Success(Unit.Value);
            }
        }
    }
}
