using System;
using System.ComponentModel.DataAnnotations;

namespace Gabinet.ViewModel
{
    public class RoleVModel
    {
     [Required(ErrorMessage = "Pole jest obowiązkowe")]
     [MinLength(3, ErrorMessage = "Minimalna długośc wynośi 3")]
     public string Name { get; set; }
    }
}
