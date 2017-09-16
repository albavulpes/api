using System;
using System.Collections.Generic;
using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Models.Resource
{
    public class ComicInfo : ApiModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string CoverImageThumbnail { get; set; }
    }
    public class Comic : ComicInfo
    {
        public string CoverImageFullSize { get; set; }

        public List<ArcInfo> Arcs { get; set; }
    }
}
