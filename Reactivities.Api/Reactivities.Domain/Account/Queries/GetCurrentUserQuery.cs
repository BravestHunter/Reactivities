using MediatR;
using Reactivities.Domain.Account.Dtos;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Account.Queries
{
    public class GetCurrentUserQuery : IRequest<Result<CurrentUserDto>>
    {
    }
}
