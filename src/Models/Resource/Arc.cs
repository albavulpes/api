using System;
using System.Collections.Generic;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Shared;
using Marten.Schema;

namespace AlbaVulpes.API.Models.Resource
{
    public class Arc : ApiModel
    {
        public Arc()
        {
            Chapters = new List<Chapter>();
        }

        [ForeignKey(typeof(Comic))]
        public Guid ComicId { get; set; }

        public string Title { get; set; }
        public int Number { get; set; }
        public Image CoverImage { get; set; }

        public List<Chapter> Chapters { get; set; }
    }
}
