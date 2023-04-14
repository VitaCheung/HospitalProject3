using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class UpdateAppointment
    {
        public AppointmentsDto SelectedAppointment { get; set; }

        public IEnumerable<StaffsDto> StaffOptions { get; set; }
        
        public IEnumerable<PatientsDto> PatientOptions { get; set; }
    }
}
