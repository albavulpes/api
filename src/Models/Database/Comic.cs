using AlbaVulpes.API.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
