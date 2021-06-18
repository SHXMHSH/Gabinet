using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Gabinet.Models
{
    public class PatientModel : PearsonModel
    {
        [Key]
        public int PatientId { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string Pesel { get; set; }

        public virtual ICollection<AppointmentModel> GetAppoitment { get; set; }


    }
}
