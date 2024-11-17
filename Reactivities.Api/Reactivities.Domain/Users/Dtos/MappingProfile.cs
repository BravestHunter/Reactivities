using AutoMapper;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Users.Dtos
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            string currentUsername = string.Empty;

            CreateMap<AppUser, ProfileShortDto>()
                .ForMember(d => d.ProfilePhotoUrl, o => o.MapFrom(s => s.ProfilePhoto != null ? s.ProfilePhoto.Url : null))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
                .ForMember(d => d.Following, o => o.MapFrom(s => s.Followers.Any(f => f.Observer.UserName == currentUsername)));
            CreateMap<AppUser, ProfileDto>()
                .ForMember(d => d.ProfilePhotoUrl, o => o.MapFrom(s => s.ProfilePhoto != null ? s.ProfilePhoto.Url : null))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
                .ForMember(d => d.Following, o => o.MapFrom(s => s.Followers.Any(f => f.Observer.UserName == currentUsername)));
            CreateMap<EditProfileDto, AppUser>();
        }
    }
}
