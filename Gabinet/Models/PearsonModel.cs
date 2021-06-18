using System;
using System.ComponentModel.DataAnnotations;

namespace Gabinet.Models
{
    //Abstract class containing detaile user data
    // UserModel
    // PatientModel
    public abstract class PearsonModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(50)]
        public string Surname { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public string Street { get; set; }
        public string City { get; set; }
        public string PostCode{get;set;}

        [Required]
        [Phone]
        public string Phone { get; set; }

        
    }
}
