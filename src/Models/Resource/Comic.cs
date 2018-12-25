using System;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Shared;

namespace AlbaVulpes.API.Models.Resource
{
    public class Comic : ApiModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime ReleaseDate { get; set; }

        public Image CoverImage { get; set; }
    }
}
