using MediatR;
using Reactivities.Domain.Account.Dtos;
using Reactivities.Domain.Core;
using Reactivities.Domain.Core.Exceptions;
using Reactivities.Domain.Core.Interfaces;
using Reactivities.Domain.Users.Interfaces;

namespace Reactivities.Domain.Account.Queries.Handlers
{
    internal class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, Result<CurrentUserDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAccessor _userAccessor;

        public GetCurrentUserHandler(IUserRepository userRepository, IUserAccessor userAccessor)
        {
            _userRepository = userRepository;
            _userAccessor = userAccessor;
        }

        public async Task<Result<CurrentUserDto>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var currentUsername = _userAccessor.GetUsername();
                var currentUserDto = await _userRepository.GetCurrentUserDto(currentUsername);
                if (currentUserDto == null)
                {
                    return Result<CurrentUserDto>.Failure(new NotFoundException("Failed to find current user"));
                }

                return Result<CurrentUserDto>.Success(currentUserDto);
            }
            catch (Exception ex)
            {
                return Result<CurrentUserDto>.Failure(ex);
            }
        }
    }
}
