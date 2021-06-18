using System;
using Microsoft.AspNetCore.Identity;

namespace Gabinet.Models
{
    //Idenetity user exntension
    public class IdentityModel : IdentityUser
    {
        [PersonalData]
        public int? UserId { get; set; }
        [PersonalData]
        public UserModel User { get; set; }
    }
}
