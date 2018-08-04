using System;
using System.Collections.Generic;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Shared;
using Marten.Schema;

namespace AlbaVulpes.API.Models.Resource
{
    public class Comic : ApiModel
    {
        public Comic()
        {
            Arcs = new List<Arc>();
        }

        public string Title { get; set; }
        public string Author { get; set; }
        public DateTime ReleaseDate { get; set; }
        public Image CoverImage { get; set; }

        public List<Arc> Arcs { get; set; }
    }
}
