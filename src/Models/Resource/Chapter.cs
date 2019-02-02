using System;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Shared;
using Marten.Schema;

namespace AlbaVulpes.API.Models.Resource
{
    public class Chapter : MediaContentCollection
    {
        [ForeignKey(typeof(Arc))]
        public Guid ArcId { get; set; }

        public int ChapterNumber { get; set; }
    }
}
