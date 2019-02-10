using AlbaVulpes.API.Models.Resource;

namespace AlbaVulpes.API.Models.Responses
{
    public class ComicResponse : Comic
    {
        public int ChaptersCount { get; set; }
    }
}