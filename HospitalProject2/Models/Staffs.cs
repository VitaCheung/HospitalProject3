using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models
{
    public class Staffs
    {
        [Key]
        public int staff_id { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }

        // each staff should be part of a department
        [ForeignKey("Departments")]
        public int department_id { get; set; }
        public virtual Departments Departments { get; set; }

        public string bio { get; set; }
        public string image { get; set; }
    }
    public class StaffsDto
    {
        public int staff_id { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public int department_id { get; set; }
        public string bio { get; set; }
        public string image { get; set; }
    }
}