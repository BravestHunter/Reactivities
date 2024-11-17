using AutoMapper;
using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Users.Dtos;

namespace Reactivities.Domain.Activities.Dtos
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            string currentUsername = string.Empty;

            CreateMap<ActivityAttendee, ProfileShortDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.User.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.User.Bio))
                .ForMember(d => d.ProfilePhotoUrl, o => o.MapFrom(s => s.User.ProfilePhoto != null ? s.User.ProfilePhoto.Url : null))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.User.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.User.Followings.Count))
                .ForMember(d => d.Following, o => o.MapFrom(s => s.User.Followers.Any(f => f.Observer.UserName == currentUsername)));
            CreateMap<Activity, ActivityDto>();
            CreateMap<CreateActivityDto, Activity>();
            CreateMap<EditActivityDto, Activity>();
            CreateMap<Activity, UserActivityDto>();
        }
    }
}
