﻿using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Models.Responses;
using AutoMapper;

namespace AlbaVulpes.API.Services
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Comic, ComicResponse>()
                .ForMember(x => x.ChaptersCount, c => c.Ignore());

            CreateMap<Arc, ArcResponse>()
                .ForMember(x => x.ChaptersCount, c => c.Ignore());

            CreateMap<Chapter, ChapterResponse>()
                .ForMember(x => x.PagesCount, c => c.Ignore());
        }
    }
}