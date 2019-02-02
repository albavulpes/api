using System;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.Shared;

namespace AlbaVulpes.API.Models.Resource
{
    public class Comic : MediaContent
    {
        public DateTime CreatedDate { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
