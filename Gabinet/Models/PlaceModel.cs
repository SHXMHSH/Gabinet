using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Gabinet.Models
{
    public class PlaceModel
    {
       [Key]
       public int PlaceId { get; set; }

        [MaxLength(100)]
        [Required]
        public string Name { get; set; }


        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }

        public bool Active { get; set; } = true;


        public virtual ICollection<UserModel> User { get; set; }

        public static implicit operator PlaceModel(Task<PlaceModel> v)
        {
            throw new NotImplementedException();
        }
    }
}
