using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject2.Models
{
    public class Services
    {
        [Key]
        public int service_id { get; set; }
        public string name { get; set; }

        // each service belongs to one program, but one program can have many services
        [ForeignKey("Program")]
        public int program_id { get; set; }
        public virtual Programs Program { get; set; }

        public string description { get; set; }
        public string location { get; set; }
    }

    public class ServicesDto
    {
        public int service_id { get; set; }
        public string name { get; set; }
        public int program_id { get; set; }
        public string description { get; set; }
        public string location { get; set; }
    }
}