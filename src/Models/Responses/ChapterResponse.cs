using AlbaVulpes.API.Models.Resource;

namespace AlbaVulpes.API.Models.View
{
    public class ChapterResponse : Chapter
    {
        public int PagesCount { get; set; }
    }
}