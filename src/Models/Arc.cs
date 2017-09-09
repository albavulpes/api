using AlbaVulpes.API.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbaVulpes.API.Models
{
    public class Arc : ApiModel
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public List<Chapter> Chapters { get; set; }
    }
}
