using System;
using System.Collections.Generic;
using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Models.Resource
{
    public class Chapter : ApiModel
    {
        public Guid ArcId { get; set; }

        public string Title { get; set; }
        public int ChapterNumber { get; set; }

        public List<Page> Pages { get; set; }
    }
}
