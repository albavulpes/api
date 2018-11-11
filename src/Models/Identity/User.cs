using System.Collections.Generic;
using AlbaVulpes.API.Base;

namespace AlbaVulpes.API.Models.Identity
{
    public class User : ApiModel
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Role> Roles { get; set; }
    }
}