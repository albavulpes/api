using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Models.View;
using AutoMapper;

namespace AlbaVulpes.API.Services
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Comic, ComicViewModel>();
            CreateMap<Arc, ArcViewModel>();
            CreateMap<Chapter, ChapterViewModel>();
        }
    }
}