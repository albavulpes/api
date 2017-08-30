using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbaVulpes.API.Models
{
    public class Chapter
    {
        public long Id { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public List<Page> Pages { get; set; }
    }
}
