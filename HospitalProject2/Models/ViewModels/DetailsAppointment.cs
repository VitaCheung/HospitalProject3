using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class DetailsAppointment
    {
        public AppointmentsDto SelectedAppointment { get; set; }
        
        public StaffsDto RelatedStaffs { get; set; }

        public PatientsDto RelatedPatients { get; set; }
    }
}
