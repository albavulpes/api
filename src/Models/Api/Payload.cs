using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlbaVulpes.API.Models.Api
{
    public class Payload<TData>
    {
        public string Error { get; set; }
        public TData Data { get; set; }
    }
}
