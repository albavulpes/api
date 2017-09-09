using AlbaVulpes.API.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlbaVulpes.API.Models.App;

namespace AlbaVulpes.API.Models.Database
{
    public class Page : ApiModel
    {
        public int Number { get; set; }
        public ImageSet Image { get; set; }
    }
}
