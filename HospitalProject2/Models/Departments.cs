using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models
{
    public class Departments
    {
        [Key]
        public int department_id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public int size { get; set; }
    }
    public class DepartmentsDto
    {
        public int department_id { get; set; }
        public string name { get; set; }
        public string location { get; set; }
        public int size { get; set; }
    }
}