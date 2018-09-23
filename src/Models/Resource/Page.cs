using System;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Shared;
using Marten.Schema;

namespace AlbaVulpes.API.Models.Resource
{
    public class Page : ApiModel
    {
        [ForeignKey(typeof(Chapter))]
        public Guid ChapterId { get; set; }

        public int PageNumber { get; set; }

        public Image Image { get; set; }
    }
}
