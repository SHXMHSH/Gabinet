using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Gabinet.Models
{
    public class UserModel : PearsonModel
    {
        [Key]
        public int UserId { get; set; }

        public int PlaceId { get; set; }
        [ForeignKey("PlaceId")]
        public virtual PlaceModel Place { get; set; }

        public string Position { get; set; }
    }
}
