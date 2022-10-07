using Microsoft.AspNetCore.Identity;

namespace AlgorArt.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string AccountAddress { get; set; }
        public string Key { get; set; }
    }
}
