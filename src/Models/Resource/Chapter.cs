using System;
using System.Collections.Generic;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Shared;
using Marten.Schema;

namespace AlbaVulpes.API.Models.Resource
{
    public class Chapter : ApiModel
    {
        public Chapter()
        {
            Pages = new List<Page>();
        }

        [ForeignKey(typeof(Arc))]
        public Guid ArcId { get; set; }

        public string Title { get; set; }
        public int ChapterNumber { get; set; }
        public Image CoverImage { get; set; }

        public List<Page> Pages { get; set; }
    }
}
