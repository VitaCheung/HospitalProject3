using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models
{
    public class Appointments
    {
        [Key]
        public int appointment_id { get; set; }
        public int health_num { get; set; }

        public DateTime date_time { get; set; }

        public string symptoms { get; set; }

        //an appointment belongs to one patient, and a patient can have more than one appointment 
        [ForeignKey("Patients")]
        public int patient_id { get; set; }

        public virtual Patients Patients { get; set; }

        [ForeignKey("Staffs")]
        public int staff_id { get; set;}

        public virtual Staffs Staffs { get; set; }

    }

    public class AppointmentsDto
    {
        public int appointment_id { get; set; }
        public int health_num { get; set; }

        public DateTime date_time { get; set; }

        public string symptoms { get; set; }

        public int patient_id { get; set; }

        public int staff_id { get; set; }

    }

}