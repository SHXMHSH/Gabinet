using System;
using System.ComponentModel.DataAnnotations;

namespace Gabinet.Models
{
    public class AppointmentModel
    {
        [Key]
        public int AppointmentId { get; set; }
        [Required]
        public int PatientId { get; set; }
        public PatientModel Patient { get; set; }

        public int? UserId { get; set; }
        public UserModel User { get; set; }

        public DateTime Date { get; set; }
        public DateTime EndAppointment { get; set; }
        
        public float? Price { get; set; }

        public int PlaceId { get; set; }
        public PlaceModel Place { get; set; }
        public bool IsAccepted { get; set; }
        public string Description { get; set; }
    }
}
