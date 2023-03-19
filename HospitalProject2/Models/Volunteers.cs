using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospitalProject2.Models
{
    public class Volunteers
    {
        [Key]
        public int volunteer_id { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string contact { get; set; }
        public string email { get; set; }
        public int program_id { get; set; }
        public int hours { get; set; }

    }

    public class VolunteersDto
    {
        
        public int volunteer_id { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string contact { get; set; }
        public string email { get; set; }
        public int program_id { get; set; }
        public int hours { get; set; }

    }
}