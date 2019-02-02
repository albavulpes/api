using System;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Shared;
using Marten.Schema;

namespace AlbaVulpes.API.Models.Resource
{
    public class Arc : MediaContentCollection
    {
        [ForeignKey(typeof(Comic))]
        public Guid ComicId { get; set; }

        public int ArcNumber { get; set; }
    }
}
