using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models.ViewModels
{
    public class UpdatePatient
    {
        public PatientsDto SelectedPatients { get; set; }

        public IEnumerable<AppointmentsDto> AppointmentOptions { get; set; }
    }
}
