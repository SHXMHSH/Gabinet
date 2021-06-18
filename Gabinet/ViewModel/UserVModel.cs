using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gabinet.Models;
using Microsoft.AspNetCore.Identity;

namespace Gabinet.ViewModel
{
    public class UserVModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public UserModel UserModel { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Role { get; set; }
        public IEnumerable<IdentityRole> RoleIdentity { get; set; }
        public IEnumerable<PlaceModel> GetPlaces { get; set; }
    }
}
