using System;
using System.ComponentModel.DataAnnotations;

namespace Gabinet.ViewModel
{
    public class LoginVModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
