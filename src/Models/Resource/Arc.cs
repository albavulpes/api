using System;
using System.Collections.Generic;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.App;

namespace AlbaVulpes.API.Models.Resource
{
    public class Arc : ApiModel
    {
        public Guid ComicId { get; set; }

        public int Number { get; set; }
        public string Title { get; set; }
        public ImageSet CoverImage { get; set; }

        public List<Chapter> Chapters { get; set; }
    }
}
