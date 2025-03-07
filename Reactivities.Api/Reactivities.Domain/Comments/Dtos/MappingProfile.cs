﻿using AutoMapper;
using Reactivities.Domain.Comments.Models;

namespace Reactivities.Domain.Comments.Dtos
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Comment, CommentDto>()
                .ForMember(d => d.Username, o => o.MapFrom(s => s.Author.UserName))
                .ForMember(d => d.DisplayName, o => o.MapFrom(s => s.Author.DisplayName))
                .ForMember(d => d.ProfilePhotoUrl, o => o.MapFrom(s => s.Author.ProfilePhoto != null ? s.Author.ProfilePhoto.Url : null));
        }
    }
}
