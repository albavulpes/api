using System.Collections.Generic;

namespace AlbaVulpes.API.Models.Identity
{
    public class User
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public List<Role> Roles { get; set; }
    }
}