using System;
using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Models.Resource
{
    public class PageInfo : ApiModel
    {
        public int PageNumber { get; set; }
        public string ImageThumbnail { get; set; }
    }

    public class Page : PageInfo
    {
        public Guid ChapterId { get; set; }
        public string ImageFullSize { get; set; }
    }
}
