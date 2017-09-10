using AlbaVulpes.API.Base;
using AlbaVulpes.API.Models.App;

namespace AlbaVulpes.API.Models.Database
{
    public class Page : ApiModel
    {
        public int Number { get; set; }
        public ImageSet Image { get; set; }
    }
}
