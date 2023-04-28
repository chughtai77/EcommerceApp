using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace ecommerceapp.Models
{
    public class Register : IdentityUser
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        //[Required]
        //public string Email { get; set; }

        [Required]
        public string Password { get; set; }


        //public string Phoneno { get; set; }
        public string Address { get; set; }

    }
}
