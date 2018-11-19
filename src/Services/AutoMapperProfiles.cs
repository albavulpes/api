using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Models.View;
using AutoMapper;

namespace AlbaVulpes.API.Services
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Comic, ComicResponse>()
                .ForMember(x => x.ArcsCount, c => c.Ignore());

            CreateMap<Arc, ArcResponse>()
                .ForMember(x => x.ChaptersCount, c => c.Ignore());

            CreateMap<Chapter, ChapterResponse>()
                .ForMember(x => x.PagesCount, c => c.Ignore());
        }
    }
}