using System;
using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.App;

namespace AlbaVulpes.API.Models.Views
{
    public class PageViewModel
    {
        public Guid Id { get; set; }
        public int PageNumber { get; set; }
        public Guid ChapterId { get; set; }

        public ImageSet Image { get; set; }
    }
}
