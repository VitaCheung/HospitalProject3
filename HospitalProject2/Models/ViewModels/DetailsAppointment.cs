using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class DetailsAppointment
    {
        public AppointmentsDto SelectedAppointment { get; set; }
        
        public StaffsDto RelatedStaff { get; set; }

        public PatientsDto RelatedPatient { get; set; }
    }
}
