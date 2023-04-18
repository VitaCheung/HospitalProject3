using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class DetailsPatient
    {
        public AppointmentsDto SelectedAppointment { get; set; }

        public PatientsDto RelatedPatient { get; set; }

    }
}
