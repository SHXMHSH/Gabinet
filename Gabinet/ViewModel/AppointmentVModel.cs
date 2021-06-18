using System;
using System.Collections.Generic;
using Gabinet.Models;

namespace Gabinet.ViewModel
{
    public class AppointmentVModel
    {

        public AppointmentModel GetAppointment { get; set; }
        public IEnumerable<PatientModel> GetPatient { get; set; }
        public IEnumerable<UserModel> GetUser { get; set; }
    }
}
