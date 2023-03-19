using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalProject2.Models
{
    public class Careers
    {
        [Key]
        public int job_id { get; set; }
        public string title { get; set; }
        public int department_id { get; set; }
        public string category { get; set; }
        public string job_type { get; set; }
        public DateTime posting_date { get; set; }
        public DateTime closing_date { get; set; }

    }

    public class CareersDto
    {
        public int job_id { get; set; }
        public string title { get; set; }
        public int department_id { get; set; }
        public string category { get; set; }
        public string job_type { get; set; }
        public DateTime posting_date { get; set; }
        public DateTime closing_date { get; set; }

    }
}