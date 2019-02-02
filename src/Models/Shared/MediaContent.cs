using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Models.Shared
{
    public class MediaContent : ApiModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }

        public Image CoverImage { get; set; }
    }
}