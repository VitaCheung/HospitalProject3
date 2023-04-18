using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class DetailsPatient
    {
        public PatientsDto SelectedPatients { get; set; }

        public AppointmentsDto RelatedAppointment { get; set; }

    }
}
