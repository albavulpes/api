using AlbaVulpes.API.Base;
using System.Collections.Generic;

namespace AlbaVulpes.API.Models.Database
{
    public class Arc : ApiModel
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public List<Chapter> Chapters { get; set; }
    }
}
