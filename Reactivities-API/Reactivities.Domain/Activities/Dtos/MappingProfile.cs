using Reactivities.Domain.Activities.Models;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Activities.Dtos
{
    public class MappingProfiles : AutoMapper.Profile
    {
        public MappingProfiles()
        {
            string currentUsername = null;

            CreateMap<AppUser, AttendeeDto>();
            CreateMap<Activity, ActivityDto>();
            CreateMap<CreateActivityDto, Activity>();
            CreateMap<EditActivityDto, Activity>();

            CreateMap<ActivityAttendee, AttendeeDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.User.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.User.DisplayName))
                .ForMember(d => d.Bio, o => o.MapFrom(s => s.User.Bio))
                .ForMember(d => d.Image, o => o.MapFrom(s => s.User.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.User.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.User.Followings.Count))
                .ForMember(d => d.Following, o => o.MapFrom(s => s.User.Followers.Any(f => f.Observer.UserName == currentUsername)));
        }
    }
}
