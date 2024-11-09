using Reactivities.Domain.Activities.Dtos;
using Reactivities.Domain.Activities.Models;

namespace Reactivities.Domain.Activities.Interfaces
{
    public interface IActivityRepository
    {
        Task<Activity?> GetByIdAsync(long id);
        Task<ActivityDto?> GetDtoByIdAsync(long id, string currentUsername);
    }
}
