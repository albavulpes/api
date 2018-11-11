using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Models.View;
using AutoMapper;

namespace AlbaVulpes.API.Services
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Comic, ComicViewModel>()
                .ForMember(x => x.ArcsCount, c => c.Ignore());

            CreateMap<Arc, ArcViewModel>()
                .ForMember(x => x.ChaptersCount, c => c.Ignore());

            CreateMap<Chapter, ChapterViewModel>()
                .ForMember(x => x.PagesCount, c => c.Ignore());
        }
    }
}