using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace JWT_Practice.Models
{
    public class AppUser: IdentityUser
    {
        public DateTime userJoined { get; set; }

        public AppUser()
        {
            this.userJoined = DateTime.Now;
        }
    }
}
