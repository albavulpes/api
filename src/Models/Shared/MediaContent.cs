using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Models.Shared
{
    public class MediaContentCollection : MediaContent
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
    }

    public class MediaContent : ApiModel
    {
        public Image CoverImage { get; set; }
    }
}