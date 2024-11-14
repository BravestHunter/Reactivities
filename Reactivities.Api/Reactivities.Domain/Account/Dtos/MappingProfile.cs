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
                .ForMember(d => d.ProfilePhotoUrl, o => o.MapFrom(s => s.ProfilePhoto != null ? s.ProfilePhoto.Url : null));
        }
    }
}
