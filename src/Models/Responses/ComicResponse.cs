using AlbaVulpes.API.Models.Resource;

namespace AlbaVulpes.API.Models.View
{
    public class ComicResponse : Comic
    {
        public int ArcsCount { get; set; }
    }
}