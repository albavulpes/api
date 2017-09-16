using System;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.App;

namespace AlbaVulpes.API.Models.Resource
{
    public class PageInfo : ApiModel
    {
        public int PageNumber { get; set; }
        public Guid ChapterId { get; set; }
    }

    public class Page : PageInfo
    {
        public ImageSet Image { get; set; }
    }
}
