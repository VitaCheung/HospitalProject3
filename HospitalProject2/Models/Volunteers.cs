using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("Programs")]
        public int program_id { get; set; }
        public virtual Programs Programs { get; set; }
        public int hours { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }

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

        public string UserId { get; set; }

    }
}