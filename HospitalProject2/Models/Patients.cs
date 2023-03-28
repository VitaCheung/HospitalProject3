using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models
{
    public class Patients
    {
        [Key]
        public int patient_id { get; set; }
        public int health_num { get; set; }

        public string f_name { get; set; }
        public string l_name { get; set; }

        public DateTime bday { get; set; }

        public string  address { get; set; }

        public string phone { get; set; }

    }
}