using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Filters;
using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Activities.Interfaces
{
    public interface IActivityRepository
    {
        Task<Activity?> GetById(long id);
        Task<Activity?> GetByIdWithAttendees(long id);
        Task<ActivityDto?> GetDtoById(long id, string currentUsername);
        Task<PagedList<ActivityDto>> GetDtoList(PagingParams pagingParams, ActivityListFilters filters, string currentUsername);
        Task<PagedList<UserActivityDto>> GetUserActivityDtoList(PagingParams pagingParams, UserActivityListFilters filters);
        Task<ActivityDto> Add(Activity activity);
        Task<ActivityDto> Update(Activity activity);
        Task Delete(Activity activity);
    }
}
