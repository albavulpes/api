using AlbaVulpes.API.Models.Resource;
using AlbaVulpes.API.Models.View;
using AutoMapper;

namespace AlbaVulpes.API.Helpers
{
    public static class AutomapperInitializer
    {
        public static void InitMaps()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Comic, ComicViewModel>();
                cfg.CreateMap<Arc, ArcViewModel>();
                cfg.CreateMap<Chapter, ChapterViewModel>();
            });
        }
    }
}