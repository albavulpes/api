using System.Collections.Generic;
using AlbaVulpes.API.Models.Resource;

namespace AlbaVulpes.API.Models.Responses
{
    public class ChapterResponse : Chapter
    {
        public int PagesCount { get; set; }
    }

    public class ChapterGroupResponse
    {
        public Arc Arc { get; set; }
        public List<ChapterResponse> Chapters { get; set; }
    }
}