using AlbaVulpes.API.Base;
using System.Collections.Generic;

namespace AlbaVulpes.API.Models.Database
{
    public class Chapter : ApiModel
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public List<Page> Pages { get; set; }
    }
}
