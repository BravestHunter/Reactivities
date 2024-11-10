using AutoMapper;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Users.Dtos
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            string currentUsername = string.Empty;

            CreateMap<AppUser, ProfileDto>()
                .ForMember(d => d.Image, o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(d => d.FollowersCount, o => o.MapFrom(s => s.Followers.Count))
                .ForMember(d => d.FollowingCount, o => o.MapFrom(s => s.Followings.Count))
                .ForMember(d => d.Following, o => o.MapFrom(s => s.Followers.Any(f => f.Observer.UserName == currentUsername)));
        }
    }
}
