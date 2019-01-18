using AlbaVulpes.API.Models.Resource;

namespace AlbaVulpes.API.Models.Responses
{
    public class ChapterResponse : Chapter
    {
        public int PagesCount { get; set; }
    }
}