using AlbaVulpes.API.Base;
using System.Collections.Generic;
using AlbaVulpes.API.Models.App;

namespace AlbaVulpes.API.Models.Database
{
    public class Comic : ApiModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public List<Arc> Arcs { get; set; }
        public ImageSet CoverImage { get; set; }
    }
}
