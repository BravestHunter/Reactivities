using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Filters;
using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Core;

namespace Reactivities.Domain.Activities.Interfaces
{
    public interface IActivityRepository
    {
        Task<Activity?> GetById(long id);
        Task<ActivityDto?> GetDtoById(long id, string currentUsername);
        Task<PagedList<ActivityDto>> GetDtoList(PagingParams pagingParams, ActivityListFilters filters, string currentUsername);
        Task<ActivityDto> Add(Activity activity);
        Task<ActivityDto> Update(Activity activity);
    }
}
