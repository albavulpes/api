using System;
using System.Collections.Generic;
using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Models.Resource
{
    public class ArcInfo : ApiModel
    {
        public string Title { get; set; }
        public int Number { get; set; }
        public string CoverImageThumbnail { get; set; }
    }

    public class Arc : ArcInfo
    {
        public Guid ComicId { get; set; }
        public string CoverImageFullSize { get; set; }

        public List<ChapterInfo> Chapters { get; set; }
    }
}
