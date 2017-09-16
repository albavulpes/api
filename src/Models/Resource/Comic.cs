using System.Collections.Generic;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.App;

namespace AlbaVulpes.API.Models.Resource
{
    public class Comic : ApiModel
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public List<ArcInfo> Arcs { get; set; }
        public ImageSet CoverImage { get; set; }
    }
}
