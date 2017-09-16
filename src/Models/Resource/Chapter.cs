using System.Collections.Generic;
using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Models.Resource
{
    public class ChapterInfo : ApiModel
    {
        public string Title { get; set; }
        public int ChapterNumber { get; set; }
        public List<PageInfo> Pages { get; set; }
    }

    public class Chapter : ChapterInfo
    {
    }
}
