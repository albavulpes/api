using System;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Shared;
using Marten.Schema;

namespace AlbaVulpes.API.Models.Resource
{
    public class Page : MediaContent
    {
        [ForeignKey(typeof(Chapter))]
        public Guid ChapterId { get; set; }

        public int PageNumber { get; set; }
    }
}
