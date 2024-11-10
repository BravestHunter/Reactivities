﻿using AutoMapper;
using Reactivities.Domain.Photos.Models;

namespace Reactivities.Domain.Photos.Dtos
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            string currentUsername = string.Empty;

            CreateMap<Photo, PhotoDto>();
        }
    }
}
