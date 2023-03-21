using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject2.Models
{
    public class Programs
    {
        [Key]
        public int program_id { get; set; }
        public string name { get; set; }

        // each program belongs to one department, but one department can have many programs
        [ForeignKey("Department")]
        public int department_id { get; set; }
        public virtual Departments Department { get; set; }
        public string description { get; set; }
    }

    public class ProgramsDto
    {
        public int program_id { get; set; }
        public string name { get; set; }
        public int department_id { get; set; }
        public string description { get; set; }
    }
}