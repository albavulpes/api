using System.Collections.Generic;
using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Models.Resource
{
    public class Arc : ApiModel
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public List<Chapter> Chapters { get; set; }
    }
}
