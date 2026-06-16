using Microsoft.AspNetCore.Identity;

namespace FoodOrderSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Role { get; set; }

        public string FullName { get; set; }

        public string Address { get; set; }
    }
}