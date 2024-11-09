using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Filters;
using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Activities.Interfaces
{
    public interface IActivityRepository
    {
        Task<Activity?> GetByIdAsync(long id);
        Task<ActivityDto?> GetDtoByIdAsync(long id, string currentUsername);
        Task<PagedList<ActivityDto>> GetDtoList(PagingParams pagingParams, ActivityListFilters filters, string currentUsername);
    }
}
