using AutoMapper;
using Reactivities.Domain.Users.Models;

namespace Reactivities.Domain.Account.Dtos
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            string currentUsername = string.Empty;

            CreateMap<AppUser, CurrentUserDto>()
                .ForMember(d => d.PhotoUrl, o => o.MapFrom(s => s.Photos.FirstOrDefault(p => p.IsMain).Url));
        }
    }
}
